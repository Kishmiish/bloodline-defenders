using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles;
    [SerializeField] private float magnification = 10f;
    [SerializeField] private int chunkSize = 20;
    [SerializeField] private int xOffset = 0;
    [SerializeField] private int yOffset = 0;
    [SerializeField] private float delayTime = 1f;
    

    void Awake(){
        GenerateChunk(0,0);
    }

    void Start()
    {
        InvokeRepeating(nameof(generateChunkOnPlayer),delayTime, delayTime);
        InvokeRepeating(nameof(deleteChunkOnPlayar), delayTime, delayTime);
    }

    void generateChunkOnPlayer(){
        GenerateChunk(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y));
    }

    void deleteChunkOnPlayar(){
        deleteChunk(Mathf.FloorToInt(player.transform.position.x),Mathf.FloorToInt(player.transform.position.y));
    }

    void GenerateChunk(int xPosition, int yPosition)
    { 
        for (int i = xPosition; i < xPosition + chunkSize; i++)
        {
            for (int j = yPosition; j < yPosition + chunkSize; j++)
            {
                float rawPerlin = Mathf.PerlinNoise((i - xOffset) / magnification, (j - yOffset) / magnification);
                float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
                float perlinNoise = clampPerlin * tiles.Length;
                if(Mathf.FloorToInt(perlinNoise) == tiles.Length){
                    perlinNoise--;
                }
                Vector3Int tilePosition = new Vector3Int(i - (chunkSize / 2), j - (chunkSize / 2), 0);
                if(tilemap.GetTile(tilePosition) == null){
                    tilemap.SetTile(tilePosition, tiles[Mathf.FloorToInt(perlinNoise)]);
                }
            }
        } 
    }

    void deleteChunk(int xPosition, int yPosition){
        Vector3Int playerPosition = tilemap.WorldToCell(player.transform.position);
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var position in bounds.allPositionsWithin){
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
            if(Math.Abs(tilePosition.x - xPosition) > chunkSize / 2 || Math.Abs(tilePosition.y - yPosition) > chunkSize / 2)
            {    
                tilemap.SetTile(tilePosition, null);
            }
        }
    }
}

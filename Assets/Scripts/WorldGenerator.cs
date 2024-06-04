using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles;
    [SerializeField] private GameObject obstaclesGroup;
    [SerializeField] private GameObject[] obstacles;
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
                Vector3Int tilePosition = new Vector3Int(i - (chunkSize / 2), j - (chunkSize / 2), 0);
                if(tilemap.GetTile(tilePosition) == null){
                    float rawPerlin = Mathf.PerlinNoise((i - xOffset) / magnification, (j - yOffset) / magnification);
                    float clampPerlin = Mathf.Clamp(rawPerlin, 0.0f, 1.0f);
                    float perlinNoise = clampPerlin * tiles.Length;
                    if(Mathf.FloorToInt(perlinNoise) == tiles.Length){
                        perlinNoise--;
                    }
                    tilemap.SetTile(tilePosition, tiles[Mathf.FloorToInt(perlinNoise)]);
                    generateObstacle(tilePosition.x, tilePosition.y);
                }
            }
        } 
    }

    void deleteChunk(int xPosition, int yPosition){
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var position in bounds.allPositionsWithin){
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
            if(Math.Abs(tilePosition.x - xPosition) > chunkSize / 2 || Math.Abs(tilePosition.y - yPosition) > chunkSize / 2)
            {   
                tilemap.SetTile(tilePosition, null);
            }
        }
    }

    void generateObstacle(int xPosition, int yPosition){
        float chance = UnityEngine.Random.Range(0f, 10f);
        if(tilemap.GetTile(new Vector3Int(xPosition,yPosition,0)) != tiles[^1]){ //tiles[^1] == tiles[tiles.length - 1]
            if(0 < chance && chance < 0.05){
                Vector3 position = new Vector3(xPosition, yPosition, 0);
                Instantiate(obstacles[0], position, Quaternion.identity, obstaclesGroup.transform);
            }
            else if(0.05 < chance && chance < 0.1){
                Vector3 position = new Vector3(xPosition, yPosition, 0);
                Instantiate(obstacles[1], position, Quaternion.identity, obstaclesGroup.transform);
            }
            else if(0.1 < chance && chance < 0.15){
                Vector3 position = new Vector3(xPosition, yPosition, 0);
                Instantiate(obstacles[2], position, Quaternion.identity, obstaclesGroup.transform);
            }
        }
    }
}

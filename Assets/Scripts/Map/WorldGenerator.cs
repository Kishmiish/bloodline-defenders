using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles;
    [SerializeField] private GameObject obstaclesGroup;
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private float magnification = 10f;
    [SerializeField] private int chunkSize = 20;
    [SerializeField] private int xOffset = 0;
    [SerializeField] private int yOffset = 0;
    [SerializeField] private float delayTime = 1f;

    private GameObject[] players;
    private GameObject[] deadPlayers;
    

    void Awake(){
        UnityEngine.Random.InitState(2000);
        GenerateChunk(0,0);
    }

    void Start()
    {
        InvokeRepeating(nameof(generateChunkOnPlayer),delayTime, delayTime);
        InvokeRepeating(nameof(deleteChunkOnPlayar), delayTime, delayTime);
    }

    void generateChunkOnPlayer(){
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            GenerateChunk(Mathf.FloorToInt(player.transform.position.x), Mathf.FloorToInt(player.transform.position.y));
        }
    }

    void deleteChunkOnPlayar(){
        deleteChunk();
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

    void deleteChunk(){
        BoundsInt bounds = tilemap.cellBounds;
        foreach (var position in bounds.allPositionsWithin){
            Vector3Int tilePosition = new Vector3Int(position.x, position.y, 0);
            bool remove = true;
            foreach (var player in players)
            {
                if(Math.Abs(tilePosition.x - player.transform.position.x) < chunkSize / 2 && Math.Abs(tilePosition.y - player.transform.position.y) < chunkSize / 2)
                {
                    remove = false;
                    break;
                }
            }
            deadPlayers = GameObject.FindGameObjectsWithTag("Dead");
            foreach (var player in deadPlayers)
            {
                if(Math.Abs(tilePosition.x - player.transform.position.x) < chunkSize / 2 && Math.Abs(tilePosition.y - player.transform.position.y) < chunkSize / 2)
                {
                    remove = false;
                    break;
                }
            }
            if(remove)
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
            else if(0.15 < chance && chance < 0.55)
            {
                Vector3 position = new Vector3(xPosition, yPosition, 0);
                Instantiate(obstacles[3], position, Quaternion.identity, obstaclesGroup.transform);
            }
        }
    }
}

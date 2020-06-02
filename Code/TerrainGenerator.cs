using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    LineRenderer path;
    List<Vector3> pathPoints;
    EdgeCollider2D pathCollider;
    List<Vector2> pathColliderPoints;
    
    float perlinseed = 0;
    public static float amplitude=2;
    public static float width=0.2f;
    public GameObject bridge;
    List<Vector3> normals;
    public GameObject rocks;
    public GameObject diamond;
    public GameObject coin;
    public int roadType=0;
    float amplitudeFactor = 1;
    public GameObject gasCan;
    static float fuelX;



  

   

    //Mesh
    Mesh roadMesh;
    List<Vector3> verts;
    List<int> tris;
    List<Vector2> uv;
    MeshFilter meshFilter;



    private void Start()
    {
        bool isBridge = Random.Range(0, 10) <5 || UIScript.raceMode;
        amplitudeFactor = 1;
        if (roadType==2)
        {
            amplitudeFactor = 2;
        }
        if(roadType == 4)
        {
            amplitudeFactor = 0.5f;
        }
        int yPerlinSeed = Random.Range(0, 1000);
        uv = new List<Vector2>();
        normals = new List<Vector3>();
        roadMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        verts = new List<Vector3>();
        tris = new List<int>();
        perlinseed = CarControl.perlinSeed;
        path = GetComponent<LineRenderer>();
        pathPoints = new List<Vector3>();
        pathColliderPoints = new List<Vector2>();
        pathCollider = GetComponent<EdgeCollider2D>();

        if (CarControl.terrainPoint == Vector3.zero)
        {

            amplitude = 2*amplitudeFactor;
            perlinseed = Random.Range(0, 1000);
            CarControl.referenceY = transform.position.y;
            pathPoints.Add(new Vector3(transform.position.x, transform.position.y + Mathf.PerlinNoise(perlinseed, yPerlinSeed) * amplitude));
            fuelX = pathPoints[pathPoints.Count - 1].x;
        }
        else
        {
            
            amplitude += (1.5f)*amplitudeFactor;
            width += 0.05f;
            pathPoints.Add(new Vector3(transform.position.x,CarControl.terrainPoint.y));
        }
   
        pathColliderPoints.Add(pathPoints[0]-transform.position-new Vector3(0,10));
        pathColliderPoints.Add(pathPoints[0]-transform.position);
        int length = Random.Range(2, 5);
       
        for (float i = 0; i <length; i+=0.005f)
        {

            if (roadType == 4)
            {
                width = Random.Range(0.07f, 0.2f);
            }
            Vector3 nextPoint;
            
           if(CarControl.terrainPoint==Vector3.zero)
                nextPoint = new Vector3(pathPoints[pathPoints.Count - 1].x,transform.position.y) + new Vector3(width, Mathf.PerlinNoise(perlinseed,yPerlinSeed)*amplitude);
           else
                nextPoint = new Vector3(pathPoints[pathPoints.Count - 1].x, CarControl.referenceY) + new Vector3(width, Mathf.PerlinNoise(perlinseed, yPerlinSeed) * amplitude);
            if (pathPoints.Count == 1)
            {
                nextPoint += new Vector3(Mathf.Tan(Mathf.PI/4) * Mathf.PerlinNoise(perlinseed, yPerlinSeed) * amplitude, 0);
            }

                pathPoints.Add(nextPoint);
            
            if (i < length / 1.05f)
            {
                if (pathPoints[pathPoints.Count - 1].x - fuelX > 100 )
                {
                    Instantiate(gasCan, pathPoints[pathPoints.Count - 1]+new Vector3(0,1,-5f), Quaternion.identity);
                    fuelX = pathPoints[pathPoints.Count - 1].x;
                }
                    if (Random.Range(0, 100) > 93 && roadType == 0)
                {
                    Instantiate(rocks, new Vector3(nextPoint.x, nextPoint.y - Random.Range(1, 4),-0.1f), Quaternion.identity);
                }
                if (Random.Range(0, 100) > 97 && !UIScript.raceMode)
                {
                    Instantiate(diamond, nextPoint + new Vector3(0, 1f,-5f), Quaternion.identity);
                }
                else if (Random.Range(0, 100) > 97 && !UIScript.raceMode)
                {
                    Instantiate(coin, nextPoint + new Vector3(0, 1f,-5f), Quaternion.identity);
                }
            }
            pathColliderPoints.Add(nextPoint-transform.position);
            perlinseed += 0.05f ;
        }
        if (!isBridge)
        {
            for(int i = 20; i > 0; i--)
            {
                pathPoints[pathPoints.Count - i] = new Vector3(pathPoints[pathPoints.Count - i].x, pathPoints[pathPoints.Count - i-1].y) + new Vector3(0, 0.05f);
                pathColliderPoints[pathColliderPoints.Count - i] = new Vector2(pathColliderPoints[pathColliderPoints.Count - i].x,pathColliderPoints[pathColliderPoints.Count - i - 1].y) + new Vector2(0, 0.05f);
            }
        }
        pathColliderPoints.Add(pathColliderPoints[pathColliderPoints.Count - 1] - new Vector2(0, 10));
        pathCollider.points = pathColliderPoints.ToArray();
        path.positionCount = pathPoints.Count;
        path.SetPositions(pathPoints.ToArray());
       

        
        for(int i = 0; i < pathPoints.Count; i++)
        {
            float fract = (float)(i+1) / (pathPoints.Count);
            verts.Add(pathPoints[i]-transform.position);
            verts.Add(new Vector3(pathPoints[i].x, -40)-transform.position);
       

            uv.Add(new Vector2(fract, 0));
            uv.Add(new Vector2(fract, 1));
        }

        for(int i = 0; i < verts.Count-2; i+=2)
        {
            tris.Add(i + 2);
            tris.Add(i + 1);
            tris.Add(i);
            tris.Add(i + 3);
            tris.Add(i + 1);
            tris.Add(i + 2);

            
            
            
        }
        roadMesh.vertices = verts.ToArray();
        roadMesh.uv = uv.ToArray();
        roadMesh.triangles = tris.ToArray();
      

       
        meshFilter.mesh = roadMesh;
        if (isBridge)
        {
            Instantiate(bridge, pathPoints[pathPoints.Count - 1], Quaternion.identity);
            CarControl.terrainPoint = pathPoints[pathPoints.Count - 1] + new Vector3(12.71f, 0);
        }
        else
        {
           
            CarControl.terrainPoint = pathPoints[pathPoints.Count - 1] + new Vector3(Random.Range(3f,5.5f), 0);
        }
        CarControl.perlinSeed = perlinseed;
      
    }


    private void Update()
    {
        if (CarControl.carPos.x - pathPoints[pathPoints.Count - 1].x > 500)
        {
            Destroy(gameObject);
        }
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private TrackSegment[] segmentPrefabs;
    private const float trackBufferLength = 2000;

    private List<TrackSegment> segmentList;
    private float nextEnd = 0;


    private void Start()
    {
        segmentList = new List<TrackSegment>();
    }
    void Update()
    {
        if(car.transform.position.z > nextEnd - trackBufferLength)
        {
            SpawnSegment();
        }
    }

    private void SpawnSegment()
    {
        int seg = Random.Range(0, segmentPrefabs.Length);

        TrackSegment newSeg = Object.Instantiate(segmentPrefabs[seg], new Vector3(0, 0, nextEnd), Quaternion.identity);
        nextEnd += newSeg.length;

        segmentList.Add(newSeg);
        ClearOldSegments();
    }

    private void ClearOldSegments()
    {
        float carZ = car.transform.position.z;

        while(segmentList.Count > 0)
        {
            if (segmentList[0].transform.position.z + segmentList[0].length < carZ - trackBufferLength)
            {
                TrackSegment removed = segmentList[0];
                segmentList.RemoveAt(0);
                Object.Destroy(removed.gameObject);
            }
            else
            {
                break;
            }
        }
    }
}

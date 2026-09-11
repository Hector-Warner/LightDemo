using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AIGridManager AIGridMgr;
    private Camera myCam;
    public bool IsTrackingWalkableNodes = false;
    public bool IsTrackingPlayerCoords = false;
    
    void Start()
    {
        myCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTrackingWalkableNodes)
        {
            ReturnIsWalkableNode();
        }
        if (IsTrackingPlayerCoords)
        {
            TrackPlayerCoords();
        }
    }

    void ReturnIsWalkableNode()
    {
        Vector3 trackingRelativeMousePos = Input.mousePosition;
        trackingRelativeMousePos.z = -myCam.transform.position.z;
        Vector3 worldMousePos = myCam.ScreenToWorldPoint(trackingRelativeMousePos);
        
        Node trackingNode = AIGridMgr.GetNodeFromWorldPos(worldMousePos);
        Debug.Log(trackingNode.isWalkable);
    }

    void TrackPlayerCoords()
    {
        Debug.Log(AIGridMgr.ConvertWorldToCoord(AIGridMgr.player.transform.position));
    }
}

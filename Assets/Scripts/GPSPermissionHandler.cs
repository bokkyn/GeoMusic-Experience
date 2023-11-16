using UnityEngine;
using UnityEngine.Android;

public class GPSPermissionHandler : MonoBehaviour
{


    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }
}

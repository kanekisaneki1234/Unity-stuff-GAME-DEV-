using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // [SerializeField]
    public Transform player;
    Vector3 Camerapos;
    // public Text textbox;
    public TextMeshProUGUI textBox;

    public float maxX=370f, minX=-55f, minY=0f, maxY=10f;
    
    

    // Start is called before the first frame update
    void Start()
    {
        // player = GameObject.FindWithTag("Player").transform;
        // textBox.text = "Health: " + Player.Health.ToString();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(!player)
        {
            return;
        }
        Camerapos = transform.position;
        // Camerapos.x = player.position.x;
        // Camerapos.y = minY;
        Camerapos.x = Mathf.Clamp(player.position.x, minX, maxX);
        Camerapos.y = Mathf.Clamp(player.position.y, minY, maxY);
        // Camerapos.y = minY;
        // if(Camerapos.x>maxX)
        // {
        //     Camerapos.x=maxX;
        // }
        // else if(Camerapos.x<minX)
        // {
        //     Camerapos.x=minX;
        // }
    //    if(Camerapos.y<player.position.y)
    //     {
    //         Camerapos.y=maxY;
    //     }
    //     else if(player.position.y>maxY)
    //     {
    //         Camerapos.y=maxY;
    //     }
    //     else
    //     {
    //         Camerapos.y=player.position.y;
    //     }
        // else if(player.position.y>minY)
        // {
        //     Camerapos.y=player.position.y;
        // }
        

        // Update the camera's y position to follow the player vertically within the defined minimum Y value
        // Camerapos.y = Mathf.Clamp(player.position.y, minY, maxY);

        // Set the camera's position to the updated position
        transform.position = Camerapos;
    }
    void Update()
    {
        textBox.text = "Health: " + Player.Health.ToString();
    }
}

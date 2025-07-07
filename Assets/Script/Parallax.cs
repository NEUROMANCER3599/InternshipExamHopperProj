using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject Cam;
    [Range(0,1)] public float ParallaxEffect;
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        Cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = Cam.transform.position.x * (1 - ParallaxEffect);
        float distance = Cam.transform.position.x * ParallaxEffect;

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (temp > startpos) startpos += length;
        else if (temp < startpos - length) startpos -= length; 
      
    }
}

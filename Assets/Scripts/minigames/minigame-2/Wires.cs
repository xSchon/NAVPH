using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wires : MonoBehaviour
{
    Vector3 startPoint;
    Vector3 startPosition;
    public SpriteRenderer wireEnd;
  
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.parent.position;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        Debug.Log("Dragging");
        Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, 0.2f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                transform.position = collider.transform.position;
                Vector3 direction2 = collider.transform.position - startPoint;
                transform.right = direction2 * transform.lossyScale.x;
                float dist2 = Vector2.Distance(startPoint, collider.transform.position);
                wireEnd.size = new Vector2(dist2, wireEnd.size.y);

                // check if the wires are the same color 
                if (transform.parent.name.Equals(collider.transform.parent.name))
                {
                    Main.Instance.AddScore();
                    Debug.Log("Correct");
                    Destroy(this);
                }
                else
                {
                    Debug.Log("Wrong");
                }
                return;
            }

        }

        transform.position = newPosition;

        Vector3 direction = newPosition - startPoint;
        transform.right = direction * transform.lossyScale.x;

        float dist = Vector2.Distance(startPoint, newPosition);
        wireEnd.size = new Vector2(dist, wireEnd.size.y);



    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        transform.position = startPosition;
        Vector3 direction = startPosition - startPoint;
        transform.right = direction * transform.lossyScale.x;
        float dist = Vector2.Distance(startPoint, startPosition);
        wireEnd.size = new Vector2(dist, wireEnd.size.y);
    }
}

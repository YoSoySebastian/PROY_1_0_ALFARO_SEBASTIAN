using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveDelay = 0.15f;
    private bool isMoving = false;

    void Update()
    {
        if (!isMoving)
        {
            Vector2Int dir = Vector2Int.zero;
            if (Input.GetKeyDown(KeyCode.W)) dir = Vector2Int.up;
            if (Input.GetKeyDown(KeyCode.S)) dir = Vector2Int.down;
            if (Input.GetKeyDown(KeyCode.A)) dir = Vector2Int.left;
            if (Input.GetKeyDown(KeyCode.D)) dir = Vector2Int.right;

            if (dir != Vector2Int.zero) TryMove(dir);
        }
    }

    void TryMove(Vector2Int dir)
    {
        Vector2 targetPos = (Vector2)transform.position + dir;
        Collider2D hit = Physics2D.OverlapPoint(targetPos);

        if (hit == null || hit.CompareTag("WinTarget"))
        {
            StartCoroutine(MoveTo(targetPos));
        }
        else if (hit.CompareTag("Box"))
        {
            BoxController box = hit.GetComponent<BoxController>();
            if (box != null && box.TryMove(dir)) StartCoroutine(MoveTo(targetPos));
        }
    }

    IEnumerator MoveTo(Vector2 target)
    {
        isMoving = true;
        transform.position = target;
        ContarPasos cp = GetComponent<ContarPasos>();
        if(cp != null) cp.RegistrarMovimiento(); 
        yield return new WaitForSeconds(moveDelay);
        isMoving = false;
    }
}
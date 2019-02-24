using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manybits
{
    public class Point : MonoBehaviour
    {
        public Vector2 position;
        public int touchCount = 0;
        public Sprite[] sprites;

        private SpriteRenderer spriteRenderer;



        public Point() { }



        public Point(float x, float y)
        {
            position = new Vector2(x, y);
        }



        public Point(Vector2 pos)
        {
            position = pos;
        }



        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }



        void Update()
        {
            position = transform.position;
        }



        public void IncTouchCount()
        {
            touchCount++;
            if (touchCount < 2)
                spriteRenderer.sprite = sprites[0];
            if (touchCount == 2)
                spriteRenderer.sprite = sprites[1];
            if (touchCount == 3)
                spriteRenderer.sprite = sprites[2];
            if (touchCount > 3)
                spriteRenderer.sprite = sprites[3];
        }
    }
}

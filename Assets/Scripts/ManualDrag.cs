using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Manybits
{
    public class ManualDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public GameManager gameManager;

        private Vector2 delta;
        private bool isTouch = false;
        private int currentTouchId = -10;
        private float zPos;

        private GameProcess gameProcess;

        private Vector2 spriteSize;
        private Rect borders;
        private Point point;
        private Vector2 pivotPoint;



        void Start()
        {
            gameProcess = gameManager.gameProcess;
            SpriteRenderer pointSprite = GetComponent<SpriteRenderer>();
            point = GetComponent<Point>();
            spriteSize = pointSprite.sprite.rect.size;
            pivotPoint = pointSprite.sprite.pivot;
            borders = gameProcess.moveArea;
        }



        public void OnPointerDown(PointerEventData eventData)
        {
            if (gameProcess.GameMode == GameMode.GM_GAME && gameProcess.isLineCross == true && eventData.pointerId <= 0)
            {
                Vector3 touch = Camera.main.ScreenToWorldPoint(eventData.position);
                delta = transform.position - touch;
                isTouch = true;
                currentTouchId = eventData.pointerId;
                zPos = transform.position.z;

                gameProcess.touchesCount++;
            }
        }



        public void OnPointerUp(PointerEventData eventData)
        {
            if (currentTouchId == eventData.pointerId)
            {
                isTouch = false;
                currentTouchId = -10;

                gameProcess.AddMove();
                point.IncTouchCount();

                gameProcess.touchesCount--;
            }
        }



        public void OnDrag(PointerEventData eventData)
        {
            if (isTouch && gameProcess.GameMode == GameMode.GM_GAME)
            {
                if (currentTouchId == eventData.pointerId)
                {
                    Vector3 newPos = Camera.main.ScreenToWorldPoint(eventData.position) + (Vector3)delta;

                    float pointScale = transform.localScale.x;
                    Vector2 pointScaledSize = spriteSize * pointScale;

                    Vector2 scaledPivot = pivotPoint * pointScale;

                    float left = pointScaledSize.x - scaledPivot.x;
                    float right = pointScaledSize.x - left;
                    float top = pointScaledSize.y - scaledPivot.y;
                    float bottom = pointScaledSize.y - top;

                    if (newPos.x < borders.xMin + left)
                        newPos.x = borders.xMin + left;
                    if (newPos.x > borders.xMax - right)
                        newPos.x = borders.xMax - right;
                    if (newPos.y < borders.yMin + bottom)
                        newPos.y = borders.yMin + bottom;
                    if (newPos.y > borders.yMax - top)
                        newPos.y = borders.yMax - top;

                    //Vector2 halfSize = pointScaledSize * 0.5f;

                    //if (newPos.x < borders.xMin + halfSize.x)
                    //    newPos.x = borders.xMin + halfSize.x;
                    //if (newPos.x > borders.xMax - halfSize.x)
                    //    newPos.x = borders.xMax - halfSize.x;
                    //if (newPos.y < borders.yMin + halfSize.y)
                    //    newPos.y = borders.yMin + halfSize.y;
                    //if (newPos.y > borders.yMax - halfSize.y)
                    //    newPos.y = borders.yMax - halfSize.y;

                    newPos.z = zPos;
                    transform.position = newPos;
                }
            }
        }
    }
}

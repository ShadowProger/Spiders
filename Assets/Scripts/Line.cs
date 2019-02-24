using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manybits
{
    public class Line : MonoBehaviour
    {
        public Point point1;
        public Point point2;

        public Material material1;
        public Material material2;

        public bool isCross = false;

        private LineRenderer lineRenderer;
        private float zPos;



        public Line() { }

        public Line(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

        // Use this for initialization
        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            zPos = transform.position.z;
        }

        // Update is called once per frame
        void Update()
        {
            lineRenderer.SetPosition(0, new Vector3(point1.position.x, point1.position.y, zPos));
            lineRenderer.SetPosition(1, new Vector3(point2.position.x, point2.position.y, zPos));
        }

        public void SetCross(bool isCross)
        {
            this.isCross = isCross;
            if (isCross)
            {
                lineRenderer.material = material1;
            }
            else
            {
                lineRenderer.material = material2;
            }
        }
    }
}

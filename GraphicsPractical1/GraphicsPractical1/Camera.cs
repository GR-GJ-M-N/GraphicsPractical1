using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GraphicsPractical1
{
    class Camera
    {
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Vector3 up;
        private Vector3 eye;
        private Vector3 focus;

        public Camera(Vector3 camEye, Vector3 camFocus, Vector3 camUp, float aspectRatio = 4.0f / 3.0f)
        {
            this.up = camUp;
            this.eye = camEye;
            this.focus = camFocus;

            this.updateViewMatrix();
            this.projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
            aspectRatio, 1.0f, 300.0f);
        }

        private void updateViewMatrix()
        {
            this.viewMatrix = Matrix.CreateLookAt(this.eye, this.focus, this.up);
        }

        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
        }
        public Matrix ProjectionMatrix
        {
            get { return this.projectionMatrix; }
        }

        public Vector3 Eye
        {
            get { return this.eye; }
            set { this.eye = value; this.updateViewMatrix(); }
        }
        public Vector3 Focus
        {
            get { return this.focus; }
            set { this.focus = value; this.updateViewMatrix(); }
        }
    }
}


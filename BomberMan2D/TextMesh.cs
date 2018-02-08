using System.Collections.Generic;
using Aiv.Fast2D;
using OpenTK;

namespace BehaviourEngine.Test
{
    public class TextMesh : Mesh
    {
        private string text;
        private Texture texture;

        private float width;
        private float height;

        private Vector4 textColor;

        private Dictionary<int, Vector4> lettersColor;

        private float horizontalSpacing;

        public TextMesh(Texture texture)
        {
            text              = "";
            this.texture      = texture;
            this.texture.SetNearest();
            width             = 1; //texture.Width /16;
            height = 1;            //texture.Height /16;
            horizontalSpacing = width;
            lettersColor      = new Dictionary<int, Vector4>();
        }

        public void SetHorizontalSpacing(float n)
        {
            horizontalSpacing = n;
            RecomputeMeshVertices(text);
        }

        public void SetTextColor(Vector4 color)
        {
            textColor = color;
        }

        public void ResetTextColors()
        {
            lettersColor.Clear();
            textColor = Vector4.Zero;
        }

        public void SetLetterColor(int index, Vector4 color)
        {
            lettersColor[index] = color;
        }

        private void UpdateLettersColors()
        {
            this.vc    = new float[this.v.Length * 2];
            int index  = 0;
            for (int i = 0; i < this.vc.Length; i += 24)
            {
                Vector4 currentColor = textColor;
                if (lettersColor.ContainsKey(index))
                {
                    currentColor = lettersColor[index];
                }
                for (int j = 0; j < 24; j += 4)
                {
                    this.vc[i + j] = currentColor.X;
                    this.vc[i + j + 1] = currentColor.Y;
                    this.vc[i + j + 2] = currentColor.Z;
                    this.vc[i + j + 3] = currentColor.W;
                }
                index++;
            }
            UpdateVertexColor();
        }

        private void RecomputeMeshVertices(string message)
        {
            int size       = message.Length;
            int horizontal = 0;
            int vertical   = 0;
            this.v         = new float[size * 3 * 2 * 2];

            int index  = 0;
            for (int i = 0; i < this.v.Length; i += 12)
            {
                if (message[index] == '\n')
                {
                    vertical++;
                    horizontal = 0;
                    index++;
                    continue;
                }

                this.v[i]     = horizontal * horizontalSpacing;
                this.v[i + 1] = vertical * height;

                this.v[i + 2] = horizontal * horizontalSpacing + width;
                this.v[i + 3] = vertical * height;

                this.v[i + 4] = horizontal * horizontalSpacing;
                this.v[i + 5] = vertical * height + height;


                this.v[i + 6] = horizontal * horizontalSpacing + width;
                this.v[i + 7] = vertical * height;

                this.v[i + 8] = horizontal * horizontalSpacing + width;
                this.v[i + 9] = vertical * height + height;

                this.v[i + 10] = horizontal * horizontalSpacing;
                this.v[i + 11] = vertical * height + height;

                horizontal++;
                index++;
            }
            UpdateVertex();

            UpdateLettersColors();

           
        }

        private void RecomputeMeshUVs(string message)
        {
            this.uv = new float[this.v.Length];
            int i   = 0;
            foreach (char letter in message)
            {
                byte index     = (byte)letter;
                int x          = index % 16;
                int y = index / 16;
                           
                float sizeX    = 1f / 16;
                float sizeY    = 1f / 16;
                           
                float top      = y * sizeY;
                float bottom   = top + sizeY;
                float left     = x * sizeX;
                float right    = left + sizeX;

                this.uv[i]     = left;
                this.uv[i + 1] = top;

                this.uv[i + 2] = right;
                this.uv[i + 3] = top;

                this.uv[i + 4] = left;
                this.uv[i + 5] = bottom;


                this.uv[i + 6] = right;
                this.uv[i + 7] = top;

                this.uv[i + 8] = right;
                this.uv[i + 9] = bottom;

                this.uv[i + 10] = left;
                this.uv[i + 11] = bottom;

                i += 12;

            }
            UpdateUV();
        }

        public void UpdateText(string message)
        {
            // do not waste cycle if the string is empty
            if (message.Length == 0)
                return;

            if (message.Length > text.Length)
            {
                RecomputeMeshVertices(message);
            }
            if (message != text)
            {
                RecomputeMeshUVs(message);
            }
            text = message;

        }

        public void DrawText()
        {
            DrawTexture(texture);
        }
    }
}

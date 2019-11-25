using Microsoft.Xna.Framework;

namespace Ursine
{
    public class GeometryFactory
    {
        public Vector2 Cart2Iso(float x, float y)
        {

            float isoX = x - y;
            float isoY = (x + y) * 0.5f;

            return new Vector2(isoX, isoY);
        }


        public Vector2 Iso2Cart(float x, float y)
        {
            //float carX = (2 * x + y) /2;
            //float carY = (2 * y - x) /2 ;

            //float carX = (x - y) / (float)1.5;
            //float carY = (x / (float)3.0 + y / (float)1.5)*-1;


            //float carX = ((y / 50f) + (x / 100f));***
            //float carY = ((-x / 100f) + (y / 50f));

            //float carX = (2.0f * y + x) * 0.5f;
            //float carY = (2.0f * y - x) * 0.5f;

            float carX = (x / 50 + y / 25) / 2;
            float carY = (y / 25 - (x / 50)) / 2;

            return new Vector2(carX, carY);
        }

    }
}

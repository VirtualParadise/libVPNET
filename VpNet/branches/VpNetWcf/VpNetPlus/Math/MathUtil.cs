using System;
using System.Collections.Generic;
using VpNet.Core.Structs;

namespace VpNetPlus.Math
{
    /// <summary>
    /// This is a class which exposes static methods for various common math functions.  Currently,
    /// the methods simply wrap the methods of the System.Math class (with the exception of a few added extras).
    /// This is in case the implementation needs to be swapped out with a faster C++ implementation, if
    /// deemed that the System.Math methods are not up to far speed wise.
    /// </summary>
    /// TODO: Add overloads for all methods for all instrinsic data types (i.e. float, short, etc).
    public sealed class MathUtil
    {
        /// <summary>
        ///		Empty private constructor.  This class has nothing but static methods/properties, so a public default
        ///		constructor should not be created by the compiler.  This prevents instance of this class from being
        ///		created.
        /// </summary>
        private MathUtil() { }

        static Random random = new Random();

        #region Constant

        public const float PI = (float)System.Math.PI;
        public const float TWO_PI = (float)System.Math.PI * 2.0f;
        public const float RADIANS_PER_DEGREE = PI / 180.0f;
        public const float DEGREES_PER_RADIAN = 180.0f / PI;

        #endregion

        #region Static Methods

        /// <summary>
        ///		Converts degrees to radians.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public static float DegreesToRadians(float degrees)
        {
            return degrees * RADIANS_PER_DEGREE;
        }

        /// <summary>
        ///		Converts radians to degrees.
        /// </summary>
        /// <param name="radians"></param>
        /// <returns></returns>
        public static float RadiansToDegrees(float radians)
        {
            return radians * DEGREES_PER_RADIAN;
        }

        /// <summary>
        ///		Returns the sine of the angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Sin(float angle)
        {
            return (float)System.Math.Sin(angle);
        }

        /// <summary>
        ///     Builds a reflection matrix for the specified plane.
        /// </summary>
        /// <param name="plane"></param>
        /// <returns></returns>
        public static Matrix4 BuildReflectionMatrix(Plane plane)
        {
            Vector3 normal = plane.Normal;

            return new Matrix4(
                -2 * normal.X * normal.X + 1, -2 * normal.X * normal.Y, -2 * normal.X * normal.Z, -2 * normal.X * plane.D,
                -2 * normal.Y * normal.X, -2 * normal.Y * normal.Y + 1, -2 * normal.Y * normal.Z, -2 * normal.Y * plane.D,
                -2 * normal.Z * normal.X, -2 * normal.Z * normal.Y, -2 * normal.Z * normal.Z + 1, -2 * normal.Z * plane.D,
                0, 0, 0, 1);
        }

        /// <summary>
        ///		Calculate a face normal, including the w component which is the offset from the origin.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static Vector4 CalculateFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 normal = CalculateBasicFaceNormal(v1, v2, v3);

            // Now set up the w (distance of tri from origin
            return new Vector4(normal.X, normal.Y, normal.Z, -(normal.Dot(v1)));
        }

        /// <summary>
        ///		Calculate a face normal, no w-information.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static Vector3 CalculateBasicFaceNormal(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            Vector3 normal = (v2 - v1).Cross(v3 - v1);
            normal.Normalize();

            return normal;
        }

        /// <summary>
        ///    Calculates the tangent space vector for a given set of positions / texture coords.
        /// </summary>
        /// <remarks>
        ///    Adapted from bump mapping tutorials at:
        ///    http://www.paulsprojects.net/tutorials/simplebump/simplebump.html
        ///    author : paul.baker@univ.ox.ac.uk
        /// </remarks>
        /// <param name="position1"></param>
        /// <param name="position2"></param>
        /// <param name="position3"></param>
        /// <param name="u1"></param>
        /// <param name="v1"></param>
        /// <param name="u2"></param>
        /// <param name="v2"></param>
        /// <param name="u3"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static Vector3 CalculateTangentSpaceVector(
            Vector3 position1, Vector3 position2, Vector3 position3, float u1, float v1, float u2, float v2, float u3, float v3)
        {

            // side0 is the vector along one side of the triangle of vertices passed in, 
            // and side1 is the vector along another side. Taking the cross product of these returns the normal.
            Vector3 side0 = position1 - position2;
            Vector3 side1 = position3 - position1;
            // Calculate face normal
            Vector3 normal = side1.Cross(side0);
            normal.Normalize();

            // Now we use a formula to calculate the tangent. 
            float deltaV0 = v1 - v2;
            float deltaV1 = v3 - v1;
            Vector3 tangent = deltaV1 * side0 - deltaV0 * side1;
            tangent.Normalize();

            // Calculate binormal
            float deltaU0 = u1 - u2;
            float deltaU1 = u3 - u1;
            Vector3 binormal = deltaU1 * side0 - deltaU0 * side1;
            binormal.Normalize();

            // Now, we take the cross product of the tangents to get a vector which 
            // should point in the same direction as our normal calculated above. 
            // If it points in the opposite direction (the dot product between the normals is less than zero), 
            // then we need to reverse the s and t tangents. 
            // This is because the triangle has been mirrored when going from tangent space to object space.
            // reverse tangents if necessary.
            Vector3 tangentCross = tangent.Cross(binormal);
            if (tangentCross.Dot(normal) < 0.0f)
            {
                tangent = -tangent;
                binormal = -binormal;
            }

            return tangent;
        }

        /// <summary>
        ///		Returns the cosine of the angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Cos(float angle)
        {
            return (float)System.Math.Cos(angle);
        }

        /// <summary>
        ///		Returns the arc cosine of the angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float ACos(float angle)
        {

            // HACK: Ok, this needs to be looked at.  The decimal precision of float values can sometimes be 
            // *slightly* off from what is loaded from .skeleton files.  In some scenarios when we end up having 
            // a cos value calculated above that is just over 1 (i.e. 1.000000012), which the ACos of is Nan, thus 
            // completly throwing off node transformations and rotations associated with an animation.
            if (angle > 1)
            {
                angle = 1.0f;
            }

            return (float)System.Math.Acos(angle);
        }

        /// <summary>
        ///		Returns the arc sine of the angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float ASin(float angle)
        {
            return (float)System.Math.Asin(angle);
        }

        /// <summary>
        ///    Inverse square root.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static float InvSqrt(float number)
        {
            return 1 / Sqrt(number);
        }

        /// <summary>
        ///		Returns the square root of a number.
        /// </summary>
        /// <remarks>This is one of the more expensive math operations.  Avoid when possible.</remarks>
        /// <param name="number"></param>
        /// <returns></returns>
        public static float Sqrt(float number)
        {
            return (float)System.Math.Sqrt(number);
        }

        /// <summary>
        ///		Returns the absolute value of the supplied number.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static float Abs(float number)
        {
            return System.Math.Abs(number);
        }

        public static bool FloatEqual(float a, float b)
        {
            return FloatEqual(a, b, .00001f);
        }

        public static bool FloatEqualTolerent(float a, float b)
        {
            return FloatEqual(a, b, .0001f);
        }

        /// <summary>
        ///     Compares float values for equality, taking into consideration
        ///     that floating point values should never be directly compared using
        ///     ==.  2 floats could be conceptually equal, but vary by a 
        ///     .000001 which would fail in a direct comparison.  To circumvent that,
        ///     a tolerance value is used to see if the difference between the 2 floats
        ///     is less than the desired amount of accuracy.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static bool FloatEqual(float a, float b, float tolerance)
        {
            if (System.Math.Abs(b - a) <= tolerance)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///		Returns the tangent of the angle.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float Tan(float angle)
        {
            return (float)System.Math.Tan(angle);
        }

        /// <summary>
        ///		Used to quickly determine the greater value between two values.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static float Max(float value1, float value2)
        {
            if (float.IsNaN(value1) || float.IsNaN(value2))
                return float.NaN;
            return (value1 > value2) ? value1 : value2;
        }

        public static float Max(float value1, float value2, float value3)
        {
            if (float.IsNaN(value1) || float.IsNaN(value2) || float.IsNaN(value3))
                return float.NaN;
            float max12 = (value1 > value2) ? value1 : value2;
            return (max12 > value3) ? max12 : value3;
        }
        public static float Min(float value1, float value2, float value3)
        {
            if (float.IsNaN(value1) || float.IsNaN(value2) || float.IsNaN(value3))
                return float.NaN;
            float min12 = (value1 < value2) ? value1 : value2;
            return (min12 < value3) ? min12 : value3;
        }

        public static float Max(params float[] vals)
        {
            if (vals.Length == 0)
                throw new ArgumentException("There must be at least one value to compare");
            float max = vals[0];
            for (int i = 1; i < vals.Length; i++)
            {
                float val = vals[i];
                if (float.IsNaN(val))
                    return float.NaN;
                if (val > max)
                    max = val;
            }
            return max;
        }

        public static float Min(params float[] vals)
        {
            if (vals.Length == 0)
                throw new ArgumentException("There must be at least one value to compare");
            float min = vals[0];
            for (int i = 1; i < vals.Length; i++)
            {
                float val = vals[i];
                if (float.IsNaN(val))
                    return float.NaN;
                if (val < min)
                    min = val;
            }
            return min;
        }

        /// <summary>
        ///		Used to quickly determine the lesser value between two values.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static float Min(float value1, float value2)
        {
            return (value1 < value2 || float.IsNaN(value1)) ? value1 : value2;
        }

        /// <summary>
        ///    Returns a random value between the specified min and max values.
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximum value.</param>
        /// <returns>A random value in the range [min,max].</returns>
        public static float RangeRandom(float min, float max)
        {
            return (max - min) * UnitRandom() + min;
        }

        /// <summary>
        ///    
        /// </summary>
        /// <returns></returns>
        public static float UnitRandom()
        {
            return (float)random.Next(Int32.MaxValue) / (float)Int32.MaxValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static float SymmetricRandom()
        {
            return 2.0f * UnitRandom() - 1.0f;
        }

        /// <summary>
        ///		Checks wether a given point is inside a triangle, in a
        ///		2-dimensional (Cartesian) space.
        /// </summary>
        /// <remarks>
        ///		The vertices of the triangle must be given in either
        ///		trigonometrical (anticlockwise) or inverse trigonometrical
        ///		(clockwise) order.
        /// </remarks>
        /// <param name="px">
        ///    The X-coordinate of the point.
        /// </param>
        /// <param name="py">
        ///    The Y-coordinate of the point.
        /// </param>
        /// <param name="ax">
        ///    The X-coordinate of the triangle's first vertex.
        /// </param>
        /// <param name="ay">
        ///    The Y-coordinate of the triangle's first vertex.
        /// </param>
        /// <param name="bx">
        ///    The X-coordinate of the triangle's second vertex.
        /// </param>
        /// <param name="by">
        ///    The Y-coordinate of the triangle's second vertex.
        /// </param>
        /// <param name="cx">
        ///    The X-coordinate of the triangle's third vertex.
        /// </param>
        /// <param name="cy">
        ///    The Y-coordinate of the triangle's third vertex.
        /// </param>
        /// <returns>
        ///    <list type="bullet">
        ///        <item>
        ///            <description><b>true</b> - the point resides in the triangle.</description>
        ///        </item>
        ///        <item>
        ///            <description><b>false</b> - the point is outside the triangle</description>
        ///         </item>
        ///     </list>
        /// </returns>
        public static bool PointInTri2D(float px, float py, float ax, float ay, float bx, float by, float cx, float cy)
        {
            float v1x, v2x, v1y, v2y;
            bool bClockwise;

            v1x = bx - ax;
            v1y = by - ay;

            v2x = px - bx;
            v2y = py - by;

            bClockwise = (v1x * v2y - v1y * v2x >= 0.0);

            v1x = cx - bx;
            v1y = cy - by;

            v2x = px - cx;
            v2y = py - cy;

            if ((v1x * v2y - v1y * v2x >= 0.0) != bClockwise)
                return false;

            v1x = ax - cx;
            v1y = ay - cy;

            v2x = px - ax;
            v2y = py - ay;

            if ((v1x * v2y - v1y * v2x >= 0.0) != bClockwise)
                return false;

            return true;
        }

        #region Intersection Methods

        /// <summary>
        ///    Tests an intersection between a ray and a box.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="box"></param>
        /// <returns>A Pair object containing whether the intersection occurred, and the distance between the 2 objects.</returns>
        public static IntersectResult Intersects(Ray ray, AxisAlignedBox box)
        {
            if (box.IsNull)
            {
                return new IntersectResult(false, 0);
            }

            float lowt = 0.0f;
            float t;
            bool hit = false;
            Vector3 hitPoint;
            Vector3 min = box.Minimum;
            Vector3 max = box.Maximum;

            // check origin inside first
            if (ray.origin > min && ray.origin < max)
            {
                return new IntersectResult(true, 0.0f);
            }

            // check each face in turn, only check closest 3

            // Min X
            if (ray.origin.X < min.X && ray.direction.X > 0)
            {
                t = (min.X - ray.origin.X) / ray.direction.X;

                if (t > 0)
                {
                    // substitue t back into ray and check bounds and distance
                    hitPoint = ray.origin + ray.direction * t;

                    if (hitPoint.Y >= min.Y && hitPoint.Y <= max.Y &&
                        hitPoint.Z >= min.Z && hitPoint.Z <= max.Z &&
                        (!hit || t < lowt))
                    {

                        hit = true;
                        lowt = t;
                    }
                }
            }

            // Max X
            if (ray.origin.X > max.X && ray.direction.X < 0)
            {
                t = (max.X - ray.origin.X) / ray.direction.X;

                if (t > 0)
                {
                    // substitue t back into ray and check bounds and distance
                    hitPoint = ray.origin + ray.direction * t;

                    if (hitPoint.Y >= min.Y && hitPoint.Y <= max.Y &&
                        hitPoint.Z >= min.Z && hitPoint.Z <= max.Z &&
                        (!hit || t < lowt))
                    {

                        hit = true;
                        lowt = t;
                    }
                }
            }

            // Min Y
            if (ray.origin.Y < min.Y && ray.direction.Y > 0)
            {
                t = (min.Y - ray.origin.Y) / ray.direction.Y;

                if (t > 0)
                {
                    // substitue t back into ray and check bounds and distance
                    hitPoint = ray.origin + ray.direction * t;

                    if (hitPoint.X >= min.X && hitPoint.X <= max.X &&
                        hitPoint.Z >= min.Z && hitPoint.Z <= max.Z &&
                        (!hit || t < lowt))
                    {

                        hit = true;
                        lowt = t;
                    }
                }
            }

            // Max Y
            if (ray.origin.Y > max.Y && ray.direction.Y < 0)
            {
                t = (max.Y - ray.origin.Y) / ray.direction.Y;

                if (t > 0)
                {
                    // substitue t back into ray and check bounds and distance
                    hitPoint = ray.origin + ray.direction * t;

                    if (hitPoint.X >= min.X && hitPoint.X <= max.X &&
                        hitPoint.Z >= min.Z && hitPoint.Z <= max.Z &&
                        (!hit || t < lowt))
                    {

                        hit = true;
                        lowt = t;
                    }
                }
            }

            // Min Z
            if (ray.origin.Z < min.Z && ray.direction.Z > 0)
            {
                t = (min.Z - ray.origin.Z) / ray.direction.Z;

                if (t > 0)
                {
                    // substitue t back into ray and check bounds and distance
                    hitPoint = ray.origin + ray.direction * t;

                    if (hitPoint.X >= min.X && hitPoint.X <= max.X &&
                        hitPoint.Y >= min.Y && hitPoint.Y <= max.Y &&
                        (!hit || t < lowt))
                    {

                        hit = true;
                        lowt = t;
                    }
                }
            }

            // Max Z
            if (ray.origin.Z > max.Z && ray.direction.Z < 0)
            {
                t = (max.Z - ray.origin.Z) / ray.direction.Z;

                if (t > 0)
                {
                    // substitue t back into ray and check bounds and distance
                    hitPoint = ray.origin + ray.direction * t;

                    if (hitPoint.X >= min.X && hitPoint.X <= max.X &&
                        hitPoint.Y >= min.Y && hitPoint.Y <= max.Y &&
                        (!hit || t < lowt))
                    {

                        hit = true;
                        lowt = t;
                    }
                }
            }

            return new IntersectResult(hit, lowt);
        }


        /// <summary>
        ///    Tests an intersection between two boxes.
        /// </summary>
        /// <param name="boxA">
        ///    The primary box.
        /// </param>
        /// <param name="boxB">
        ///    The box to test intersection with boxA.
        /// </param>
        /// <returns>
        ///    <list type="bullet">
        ///        <item>
        ///            <description>None - There was no intersection between the 2 boxes.</description>
        ///        </item>
        ///        <item>
        ///            <description>Contained - boxA is fully within boxB.</description>
        ///         </item>
        ///        <item>
        ///            <description>Contains - boxB is fully within boxA.</description>
        ///         </item>
        ///        <item>
        ///            <description>Partial - boxA is partially intersecting with boxB.</description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// Submitted by: romout
        public static Intersection Intersects(AxisAlignedBox boxA, AxisAlignedBox boxB)
        {
            // grab the max and mix vectors for both boxes for comparison
            Vector3 minA = boxA.Minimum;
            Vector3 maxA = boxA.Maximum;
            Vector3 minB = boxB.Minimum;
            Vector3 maxB = boxB.Maximum;

            if ((minB.X < minA.X) &&
                (maxB.X > maxA.X) &&
                (minB.Y < minA.Y) &&
                (maxB.Y > maxA.Y) &&
                (minB.Z < minA.Z) &&
                (maxB.Z > maxA.Z))
            {

                // boxA is within boxB
                return Intersection.Contained;
            }

            if ((minB.X > minA.X) &&
                (maxB.X < maxA.X) &&
                (minB.Y > minA.Y) &&
                (maxB.Y < maxA.Y) &&
                (minB.Z > minA.Z) &&
                (maxB.Z < maxA.Z))
            {

                // boxB is within boxA
                return Intersection.Contains;
            }

            if ((minB.X > maxA.X) ||
                (minB.Y > maxA.Y) ||
                (minB.Z > maxA.Z) ||
                (maxB.X < minA.X) ||
                (maxB.Y < minA.Y) ||
                (maxB.Z < minA.Z))
            {

                // not interesting at all
                return Intersection.None;
            }

            // if we got this far, they are partially intersecting
            return Intersection.Partial;
        }


        public static IntersectResult Intersects(Ray ray, Sphere sphere)
        {
            return Intersects(ray, sphere, false);
        }

        /// <summary>
        ///		Ray/Sphere intersection test.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="sphere"></param>
        /// <param name="discardInside"></param>
        /// <returns>Struct that contains a bool (hit?) and distance.</returns>
        public static IntersectResult Intersects(Ray ray, Sphere sphere, bool discardInside)
        {
            Vector3 rayDir = ray.Direction;
            //Adjust ray origin relative to sphere center
            Vector3 rayOrig = ray.Origin - sphere.Center;
            float radius = sphere.Radius;

            // check origin inside first
            if ((rayOrig.LengthSquared <= radius * radius) && discardInside)
            {
                return new IntersectResult(true, 0);
            }

            // mmm...sweet quadratics
            // Build coeffs which can be used with std quadratic solver
            // ie t = (-b +/- sqrt(b*b* + 4ac)) / 2a
            float a = rayDir.Dot(rayDir);
            float b = 2 * rayOrig.Dot(rayDir);
            float c = rayOrig.Dot(rayOrig) - (radius * radius);

            // calc determinant
            float d = (b * b) - (4 * a * c);

            if (d < 0)
            {
                // no intersection
                return new IntersectResult(false, 0);
            }
            else
            {
                // BTW, if d=0 there is one intersection, if d > 0 there are 2
                // But we only want the closest one, so that's ok, just use the 
                // '-' version of the solver
                float t = (-b - MathUtil.Sqrt(d)) / (2 * a);

                if (t < 0)
                {
                    t = (-b + MathUtil.Sqrt(d)) / (2 * a);
                }

                return new IntersectResult(true, t);
            }
        }

        /// <summary>
        ///		Ray/Plane intersection test.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="plane"></param>
        /// <returns>Struct that contains a bool (hit?) and distance.</returns>
        public static IntersectResult Intersects(Ray ray, Plane plane)
        {
            float denom = plane.Normal.Dot(ray.Direction);

            if (MathUtil.Abs(denom) < float.Epsilon)
            {
                // Parellel
                return new IntersectResult(false, 0);
            }
            else
            {
                float nom = plane.Normal.Dot(ray.Origin) + plane.D;
                float t = -(nom / denom);
                return new IntersectResult(t >= 0, t);
            }
        }

        /// <summary>
        ///		Sphere/Box intersection test.
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="box"></param>
        /// <returns>True if there was an intersection, false otherwise.</returns>
        public static bool Intersects(Sphere sphere, AxisAlignedBox box)
        {
            if (box.IsNull) return false;

            // Use splitting planes
            Vector3 center = sphere.Center;
            float radius = sphere.Radius;
            Vector3 min = box.Minimum;
            Vector3 max = box.Maximum;

            // just test facing planes, early fail if sphere is totally outside
            if (center.X < min.X &&
                min.X - center.X > radius)
            {
                return false;
            }
            if (center.X > max.X &&
                center.X - max.X > radius)
            {
                return false;
            }

            if (center.Y < min.Y &&
                min.Y - center.Y > radius)
            {
                return false;
            }
            if (center.Y > max.Y &&
                center.Y - max.Y > radius)
            {
                return false;
            }

            if (center.Z < min.Z &&
                min.Z - center.Z > radius)
            {
                return false;
            }
            if (center.Z > max.Z &&
                center.Z - max.Z > radius)
            {
                return false;
            }

            // Must intersect
            return true;
        }

        /// <summary>
        ///		Plane/Box intersection test.
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="box"></param>
        /// <returns>True if there was an intersection, false otherwise.</returns>
        public static bool Intersects(Plane plane, AxisAlignedBox box)
        {
            if (box.IsNull) return false;

            // Get corners of the box
            Vector3[] corners = box.Corners;

            // Test which side of the plane the corners are
            // Intersection occurs when at least one corner is on the 
            // opposite side to another
            PlaneSide lastSide = plane.GetSide(corners[0]);

            for (int corner = 1; corner < 8; corner++)
            {
                if (plane.GetSide(corners[corner]) != lastSide)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///		Sphere/Plane intersection test.
        /// </summary>
        /// <param name="sphere"></param>
        /// <param name="plane"></param>
        /// <returns>True if there was an intersection, false otherwise.</returns>
        public static bool Intersects(Sphere sphere, Plane plane)
        {
            return MathUtil.Abs(plane.Normal.Dot(sphere.Center)) <= sphere.Radius;
        }

        /// <summary>
        ///    Ray/PlaneBoundedVolume intersection test.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="volume"></param>
        /// <returns>Struct that contains a bool (hit?) and distance.</returns>
        public static IntersectResult Intersects(Ray ray, PlaneBoundedVolume volume)
        {
            List<Plane> planes = volume.planes;

            float maxExtDist = 0.0f;
            float minIntDist = float.PositiveInfinity;

            float dist, denom, nom;

            for (int i = 0; i < planes.Count; i++)
            {
                Plane plane = planes[i];

                denom = plane.Normal.Dot(ray.Direction);
                if (MathUtil.Abs(denom) < float.Epsilon)
                {
                    // Parallel
                    if (plane.GetSide(ray.Origin) == volume.outside)
                        return new IntersectResult(false, 0);

                    continue;
                }

                nom = plane.Normal.Dot(ray.Origin) + plane.D;
                dist = -(nom / denom);

                if (volume.outside == PlaneSide.Negative)
                    nom = -nom;

                if (dist > 0.0f)
                {
                    if (nom > 0.0f)
                    {
                        if (maxExtDist < dist)
                            maxExtDist = dist;
                    }
                    else
                    {
                        if (minIntDist > dist)
                            minIntDist = dist;
                    }
                }
                else
                {
                    //Ray points away from plane
                    if (volume.outside == PlaneSide.Negative)
                        denom = -denom;

                    if (denom > 0.0f)
                        return new IntersectResult(false, 0);
                }
            }

            if (maxExtDist > minIntDist)
                return new IntersectResult(false, 0);

            return new IntersectResult(true, maxExtDist);
        }

        #endregion Intersection Methods

        #endregion Static Methods

    }

    /// <summary>
    ///		Simple struct to allow returning a complex intersection result.
    /// </summary>
    public struct IntersectResult
    {
        #region Fields

        /// <summary>
        ///		Did the intersection test result in a hit?
        /// </summary>
        public bool Hit;

        /// <summary>
        ///		If Hit was true, this will hold a query specific distance value.
        ///		i.e. for a Ray-Box test, the distance will be the distance from the start point
        ///		of the ray to the point of intersection.
        /// </summary>
        public float Distance;

        #endregion Fields

        /// <summary>
        ///		Constructor.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="distance"></param>
        public IntersectResult(bool hit, float distance)
        {
            this.Hit = hit;
            this.Distance = distance;
        }
    }
}
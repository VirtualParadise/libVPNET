using System;
using System.ComponentModel;
using System.Diagnostics;
using VpNet.Core.Structs;

namespace VpNetPlus.Math
{
    /// <summary>
    ///		A 3D box aligned with the x/y/z axes.
    /// </summary>
    /// <remarks>
    ///		This class represents a simple box which is aligned with the
    ///	    axes. Internally it only stores 2 points as the extremeties of
    ///	    the box, one which is the minima of all 3 axes, and the other
    ///	    which is the maxima of all 3 axes. This class is typically used
    ///	    for an axis-aligned bounding box (AABB) for collision and
    ///	    visibility determination.
    /// </remarks>

    [TypeConverter(typeof(AxisAlignedBoxConverter))]
    public sealed class AxisAlignedBox : ICloneable
    {
        #region Fields

        internal Vector3 minVector = new Vector3(-0.5f, -0.5f, -0.5f);
        internal Vector3 maxVector = new Vector3(0.5f, 0.5f, 0.5f);
        private Vector3[] corners = new Vector3[8];
        private bool isNull = true;
        private static readonly AxisAlignedBox nullBox = new AxisAlignedBox();

        #endregion

        #region Constructors

        public AxisAlignedBox()
        {
            SetExtents(minVector, maxVector);
        }

        public AxisAlignedBox(Vector3 min, Vector3 max)
        {
            SetExtents(min, max);
        }

        #endregion

        #region Public methods

        public Vector3 Size
        {
            get { return maxVector - minVector; }
            set
            {
                Vector3 center = Center;
                Vector3 halfSize = .5f * value;
                minVector = center - halfSize;
                maxVector = center + halfSize;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        public void Transform(Matrix4 matrix)
        {
            // do nothing for a null box
            if (isNull)
                return;

            Vector3 min = new Vector3();
            Vector3 max = new Vector3();
            Vector3 temp = new Vector3();

            bool isFirst = true;
            int i;

            for (i = 0; i < corners.Length; i++)
            {
                // Transform and check extents
                temp = matrix * corners[i];
                if (isFirst || temp.X > max.X)
                    max.X = temp.X;
                if (isFirst || temp.Y > max.Y)
                    max.Y = temp.Y;
                if (isFirst || temp.Z > max.Z)
                    max.Z = temp.Z;
                if (isFirst || temp.X < min.X)
                    min.X = temp.X;
                if (isFirst || temp.Y < min.Y)
                    min.Y = temp.Y;
                if (isFirst || temp.Z < min.Z)
                    min.Z = temp.Z;

                isFirst = false;
            }

            SetExtents(min, max);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCorners()
        {
            // The order of these items is, using right-handed co-ordinates:
            // Minimum Z face, starting with Min(all), then anticlockwise
            //   around face (looking onto the face)
            // Maximum Z face, starting with Max(all), then anticlockwise
            //   around face (looking onto the face)
            corners[0] = minVector;
            corners[1].X = minVector.X; corners[1].Y = maxVector.Y; corners[1].Z = minVector.Z;
            corners[2].X = maxVector.X; corners[2].Y = maxVector.Y; corners[2].Z = minVector.Z;
            corners[3].X = maxVector.X; corners[3].Y = minVector.Y; corners[3].Z = minVector.Z;

            corners[4] = maxVector;
            corners[5].X = minVector.X; corners[5].Y = maxVector.Y; corners[5].Z = maxVector.Z;
            corners[6].X = minVector.X; corners[6].Y = minVector.Y; corners[6].Z = maxVector.Z;
            corners[7].X = maxVector.X; corners[7].Y = minVector.Y; corners[7].Z = maxVector.Z;
        }

        /// <summary>
        ///		Sets both Minimum and Maximum at once, so that UpdateCorners only
        ///		needs to be called once as well.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetExtents(Vector3 min, Vector3 max)
        {
            isNull = false;

            minVector = min;
            maxVector = max;

            UpdateCorners();
        }

        /// <summary>
        ///    Scales the size of the box by the supplied factor.
        /// </summary>
        /// <param name="factor">Factor of scaling to apply to the box.</param>
        public void Scale(Vector3 factor)
        {
            Vector3 min = minVector * factor;
            Vector3 max = maxVector * factor;

            SetExtents(min, max);
        }

        #endregion

        #region Intersection Methods

        /// <summary>
        ///		Returns whether or not this box intersects another.
        /// </summary>
        /// <param name="box2"></param>
        /// <returns>True if the 2 boxes intersect, false otherwise.</returns>
        public bool Intersects(AxisAlignedBox box2)
        {
            // Early-fail for nulls
            if (this.IsNull || box2.IsNull)
                return false;

            // Use up to 6 separating planes
            if (this.maxVector.X < box2.minVector.X)
                return false;
            if (this.maxVector.Y < box2.minVector.Y)
                return false;
            if (this.maxVector.Z < box2.minVector.Z)
                return false;

            if (this.minVector.X > box2.maxVector.X)
                return false;
            if (this.minVector.Y > box2.maxVector.Y)
                return false;
            if (this.minVector.Z > box2.maxVector.Z)
                return false;

            // otherwise, must be intersecting
            return true;
        }

        /// <summary>
        ///		Tests whether this box intersects a sphere.
        /// </summary>
        /// <param name="sphere"></param>
        /// <returns>True if the sphere intersects, false otherwise.</returns>
        public bool Intersects(Sphere sphere)
        {
            return MathUtil.Intersects(sphere, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plane"></param>
        /// <returns>True if the plane intersects, false otherwise.</returns>
        public bool Intersects(Plane plane)
        {
            return MathUtil.Intersects(plane, this);
        }

        /// <summary>
        ///		Tests whether the vector point is within this box.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>True if the vector is within this box, false otherwise.</returns>
        public bool Intersects(Vector3 vector)
        {
            return (vector.X >= minVector.X && vector.X <= maxVector.X &&
                    vector.Y >= minVector.Y && vector.Y <= maxVector.Y &&
                    vector.Z >= minVector.Z && vector.Z <= maxVector.Z);
        }

        #endregion Intersection Methods

        #region Properties

        /// <summary>
        ///    Gets the center point of this bounding box.
        /// </summary>
        public Vector3 Center
        {
            get
            {
                return (minVector + maxVector) * 0.5f;
            }
            set
            {
                Vector3 halfSize = .5f * Size;
                minVector = value - halfSize;
                maxVector = value + halfSize;
            }
        }

        /// <summary>
        ///		Gets/Sets the maximum corner of the box.
        /// </summary>
        public Vector3 Maximum
        {
            get
            {
                return maxVector;
            }
            set
            {
                isNull = false;
                maxVector = value;
                UpdateCorners();
            }
        }

        /// <summary>
        ///		Gets/Sets the minimum corner of the box.
        /// </summary>
        public Vector3 Minimum
        {
            get
            {
                return minVector;
            }
            set
            {
                isNull = false;
                minVector = value;
                UpdateCorners();
            }
        }

        /// <summary>
        ///		Returns an array of 8 corner points, useful for
        ///		collision vs. non-aligned objects.
        ///	 </summary>
        ///	 <remarks>
        ///		If the order of these corners is important, they are as
        ///		follows: The 4 points of the minimum Z face (note that
        ///		because we use right-handed coordinates, the minimum Z is
        ///		at the 'back' of the box) starting with the minimum point of
        ///		all, then anticlockwise around this face (if you are looking
        ///		onto the face from outside the box). Then the 4 points of the
        ///		maximum Z face, starting with maximum point of all, then
        ///		anticlockwise around this face (looking onto the face from
        ///		outside the box). Like this:
        ///		<pre>
        ///		   1-----2
        ///		  /|    /|
        ///		 / |   / |
        ///		5-----4  |
        ///		|  0--|--3
        ///		| /   | /
        ///		|/    |/
        ///		6-----7
        ///		</pre>
        /// </remarks>
        public Vector3[] Corners
        {
            get
            {
                Debug.Assert(isNull != true, "Cannot get the corners of a null box.");

                // return a clone of the array (not the original)
                return (Vector3[])corners.Clone();
                //return corners;
            }
        }

        /// <summary>
        ///		Gets/Sets the value of whether this box is null (i.e. not dimensions, etc).
        /// </summary>
        public bool IsNull
        {
            get
            {
                return isNull;
            }
            set
            {
                isNull = value;
            }
        }

        /// <summary>
        ///		Returns a null box
        /// </summary>
        public static AxisAlignedBox Null
        {
            get
            {
                AxisAlignedBox nullBox = new AxisAlignedBox();
                // make sure it is set to null
                nullBox.IsNull = true;

                return nullBox;
            }
        }

        #endregion

        #region Operator Overloads

        public static bool operator ==(AxisAlignedBox left, AxisAlignedBox right)
        {
            return left.isNull && right.isNull ||
                   (left.corners[0] == right.corners[0] && left.corners[1] == right.corners[1] && left.corners[2] == right.corners[2] &&
                    left.corners[3] == right.corners[3] && left.corners[4] == right.corners[4] && left.corners[5] == right.corners[5] &&
                    left.corners[6] == right.corners[6] && left.corners[7] == right.corners[7]);
        }

        public static bool operator !=(AxisAlignedBox left, AxisAlignedBox right)
        {
            return left.isNull != right.isNull ||
                   (left.corners[0] != right.corners[0] || left.corners[1] != right.corners[1] || left.corners[2] != right.corners[2] ||
                    left.corners[3] != right.corners[3] || left.corners[4] != right.corners[4] || left.corners[5] != right.corners[5] ||
                    left.corners[6] != right.corners[6] || left.corners[7] != right.corners[7]);
        }
        public override bool Equals(object obj)
        {
            return obj is AxisAlignedBox && this == (AxisAlignedBox)obj;
        }

        public override int GetHashCode()
        {
            if (isNull)
                return 0;
            return corners[0].GetHashCode() ^ corners[1].GetHashCode() ^ corners[2].GetHashCode() ^ corners[3].GetHashCode() ^ corners[4].GetHashCode() ^
                   corners[5].GetHashCode() ^ corners[6].GetHashCode() ^ corners[7].GetHashCode();
        }


        public override string ToString()
        {
            return this.minVector.ToString() + ":" + this.maxVector.ToString();
        }

        public static AxisAlignedBox Parse(string text)
        {
            string[] parts = text.Split(':');
            return new AxisAlignedBox(Vector3.Parse(parts[0]), Vector3.Parse(parts[1]));
        }
        public static AxisAlignedBox FromDimensions(Vector3 center, Vector3 size)
        {
            Vector3 halfSize = .5f * size;
            return new AxisAlignedBox(center - halfSize, center + halfSize);
        }


        /// <summary>
        ///		Allows for merging two boxes together (combining).
        /// </summary>
        /// <param name="box">Source box.</param>
        public void Merge(AxisAlignedBox box)
        {
            // nothing to merge with in this case, just return
            if (box.IsNull)
            {
                return;
            }
            else if (isNull)
            {
                SetExtents(box.Minimum, box.Maximum);
            }
            else
            {
                Vector3 min = minVector;
                Vector3 max = maxVector;
                min.Floor(box.Minimum);
                max.Ceil(box.Maximum);

                SetExtents(min, max);
            }
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            return new AxisAlignedBox(this.minVector, this.maxVector);
        }

        #endregion
    }
}
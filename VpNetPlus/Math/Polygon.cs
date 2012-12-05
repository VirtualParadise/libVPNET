using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace VpNetPlus.Math
{
    /// <summary>
    /// a 2 dimensional polygon.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Polygon : ICloneable
    {
        private readonly List<Vector2> _points = new List<Vector2>();

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        public Polygon()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> class.
        /// </summary>
        /// <param name="points">A list containing <see cref="Vector2"/> instances.</param>
        public Polygon(IEnumerable<Vector2> points)
        {
            _points.AddRange(points);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Polygon"/> classu sing coordinates from another instance.
        /// </summary>
        /// <param name="polygon">A <see cref="Polygon"/> instance.</param>
        public Polygon(Polygon polygon)
        {
            _points.AddRange(polygon._points);
        }
        #endregion

        /// <summary>
        /// Gets the polygon's list of points.
        /// </summary>
        [XmlArrayItem(Type = typeof(Vector2))]
        public List<Vector2> Points
        {
            get { return _points; }
        }

        /// <summary>
        /// Gets the number of vertices in the polygon.
        /// </summary>
        public int Count
        {
            get
            {
                return _points.Count;
            }
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Polygon"/> object.
        /// </summary>
        /// <returns>The <see cref="Polygon"/> object this method creates, cast as an object.</returns>
        object ICloneable.Clone()
        {
            return new Polygon(this);
        }

        /// <summary>
        /// Creates an exact copy of this <see cref="Polygon"/> object.
        /// </summary>
        /// <returns>The <see cref="Polygon"/> object this method creates.</returns>
        public Polygon Clone()
        {
            return new Polygon(this);
        }

        /// <summary>
        /// Flips the polygon.
        /// </summary>
        public void Flip()
        {
            _points.Reverse();
        }
    }
}

using System.Collections.Generic;
using System.Linq;

namespace AvalonAssets.Rojy
{
    /// <summary>
    ///     <para>
    ///         <see cref="ShadowCast" /> handles <see cref="AngleInterval" /> used during shadow casting.
    ///     </para>
    /// </summary>
    internal class ShadowCast
    {
        private readonly SortedList<double, AngleInterval> _shadowList;

        /// <summary>
        ///     <para>
        ///         Initializes a new instance of <see cref="ShadowCast" />.
        ///     </para>
        /// </summary>
        public ShadowCast()
        {
            _shadowList = new SortedList<double, AngleInterval>();
        }

        /// <summary>
        ///     <para>
        ///         Adds a shadow with given angle.
        ///     </para>
        /// </summary>
        /// <param name="startAngle">Starting Angle.</param>
        /// <param name="endAngle">Ending Angle.</param>
        public void AddShadow(double startAngle, double endAngle)
        {
            var shadow = new AngleInterval(startAngle, endAngle);
            _shadowList.Add(startAngle, shadow);
            MergeShadow();
        }

        /// <summary>
        ///     Checks if an angle is hidden in shadow.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public bool Hide(double angle)
        {
            return _shadowList.Values.Any(shadow => shadow.Inside(angle));
        }

        private void MergeShadow()
        {
            for (var i = 0; i < _shadowList.Count; i++)
            {
                if (i + 1 >= _shadowList.Count) continue;
                AngleInterval shadow;
                // Continue if it cannot merge
                // Although it only check i & i+1, but _shadowList is sorted. 
                // Thus, it should be fine.
                if (!_shadowList.Values[i].TryMerge(_shadowList.Values[i + 1], out shadow)) continue;
                _shadowList[i] = shadow;
                _shadowList.RemoveAt(i + 1);
                return;
            }
        }
    }
}
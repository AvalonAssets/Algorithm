using System;
using System.Linq;
using AvalonAssets.Core.Utility;

namespace AvalonAssets.Rojy
{
    /// <summary>
    ///     Represents an angle range. Use to check if a angle is within the range.
    /// </summary>
    internal class AngleInterval
    {
        private readonly double _from, _to;
        private readonly bool _full;

        /// <summary>
        ///     Initialize an <see cref="AngleInterval" /> with any given angle.
        ///     The angles should should be small to have better performance.
        ///     Angle between -360 and 360 are the best.
        /// </summary>
        /// <param name="from">Starting angle.</param>
        /// <param name="to">Ending angle.</param>
        public AngleInterval(double from, double to)
        {
            _from = NormalizeAngle(from);
            _to = NormalizeAngle(to);
            _full = false;
        }

        /// <summary>
        ///     Initialize an <see cref="AngleInterval" /> with full or no interval.
        /// </summary>
        /// <param name="full">True if full interval.</param>
        public AngleInterval(bool full)
        {
            _from = 0;
            _to = 0;
            _full = full;
        }

        /// <summary>
        ///     Checks if <paramref name="target" /> is inside the interval.
        /// </summary>
        /// <param name="target">Angle to be checked.</param>
        /// <returns>True if <paramref name="target" /> is inside the interval.</returns>
        public bool Inside(double target)
        {
            if (_full)
                return true;
            if (_from < _to)
                return _from.AboutLesserThanOrEqual(target) && target.AboutLesserThanOrEqual(_to);
            return _from.AboutLesserThanOrEqual(target) || target.AboutLesserThanOrEqual(_to);
        }

        /// <summary>
        ///     Tries to merge two <see cref="AngleInterval" />.
        /// </summary>
        /// <param name="newInterval"><see cref="AngleInterval" /> to be merged.</param>
        /// <param name="resultInterval">Result <see cref="AngleInterval" /> if merge is success.</param>
        /// <returns>True if merge is success.</returns>
        public bool TryMerge(AngleInterval newInterval, out AngleInterval resultInterval)
        {
            if (newInterval._full)
            {
                resultInterval = newInterval;
                return true;
            }
            if (_full || Equals(newInterval))
            {
                resultInterval = this;
                return true;
            }
            var aStart = _from;
            var aEnd = aStart > _to ? _to + 360 : _to;
            var bStart = newInterval._from;
            var bEnd = bStart > newInterval._to ? newInterval._to + 360 : newInterval._to;
            var diffA = (aEnd - aStart)/2;
            var diffB = (bEnd - bStart)/2;
            var avgA = (aStart + aEnd)/2;
            var avgB = (bStart + bEnd)/2;
            var cosDiffA = Math.Cos(diffA.ToRadians());
            var cosDiffB = Math.Cos(diffB.ToRadians());
            var resultFlag = MergeFlag.None;
            if (Math.Cos((avgA - bStart).ToRadians()).AboutGreaterThanOrEqual(cosDiffA))
                resultFlag |= MergeFlag.BStartInsideA;
            if (Math.Cos((avgA - bEnd).ToRadians()).AboutGreaterThanOrEqual(cosDiffA))
                resultFlag |= MergeFlag.BEndInsideA;
            if (Math.Cos((avgB - aStart).ToRadians()).AboutGreaterThanOrEqual(cosDiffB))
                resultFlag |= MergeFlag.AStartInsideB;
            if (Math.Cos((avgB - aEnd).ToRadians()).AboutGreaterThanOrEqual(cosDiffB))
                resultFlag |= MergeFlag.AEndInsideB;
            if (NotHasFlags(resultFlag, MergeFlag.BStartInsideA, MergeFlag.BEndInsideA,
                MergeFlag.AEndInsideB, MergeFlag.AStartInsideB))
            {
                resultInterval = null;
                return false;
            }
            if (HasFlags(resultFlag, MergeFlag.BStartInsideA, MergeFlag.BEndInsideA,
                MergeFlag.AEndInsideB, MergeFlag.AStartInsideB))
            {
                resultInterval = new AngleInterval(true);
                return true;
            }
            if (HasFlags(resultFlag, MergeFlag.BStartInsideA, MergeFlag.BEndInsideA))
            {
                resultInterval = this;
                return true;
            }
            if (HasFlags(resultFlag, MergeFlag.AEndInsideB, MergeFlag.AStartInsideB))
            {
                resultInterval = newInterval;
                return true;
            }
            if (HasFlags(resultFlag, MergeFlag.AEndInsideB, MergeFlag.BStartInsideA) &&
                NotHasFlags(resultFlag, MergeFlag.AStartInsideB, MergeFlag.BEndInsideA))
            {
                resultInterval = new AngleInterval(aStart, bEnd);
                return true;
            }

            // Invert if
            if (!HasFlags(resultFlag, MergeFlag.AStartInsideB, MergeFlag.BEndInsideA) ||
                !NotHasFlags(resultFlag, MergeFlag.AEndInsideB, MergeFlag.BStartInsideA))
                throw new InvalidOperationException("This should never happen.");
            resultInterval = new AngleInterval(bStart, aEnd);
            return true;
        }

        public override bool Equals(object obj)
        {
            var shadow = obj as AngleInterval;
            return shadow != null && Equals(shadow);
        }

        public bool Equals(AngleInterval shadow)
        {
            return _from.AboutEqual(shadow._from) && _to.AboutEqual(shadow._to) && _full == shadow._full;
        }

        public override string ToString()
        {
            return $"From: {_from}, To: {_to}, Full: {_full}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_from.GetHashCode()*379 + _to.GetHashCode())*477 ^ _full.GetHashCode();
            }
        }

        private static bool HasFlags(MergeFlag resultFlag, params MergeFlag[] flags)
        {
            return flags.All(flag => (resultFlag & flag) == flag);
        }

        private static bool NotHasFlags(MergeFlag resultFlag, params MergeFlag[] flags)
        {
            return flags.All(flag => (resultFlag & flag) != flag);
        }

        /// <summary>
        ///     Returns angle where 0 &lt;= x &lt; 360.
        /// </summary>
        /// <param name="angle">Double in degree.</param>
        /// <returns>Normalized angle.</returns>
        private static double NormalizeAngle(double angle)
        {
            while (angle >= 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }

        [Flags]
        private enum MergeFlag : short
        {
            None = 0,
            BStartInsideA = 1 << 0,
            BEndInsideA = 1 << 1,
            AStartInsideB = 1 << 2,
            AEndInsideB = 1 << 3
        }
    }
}
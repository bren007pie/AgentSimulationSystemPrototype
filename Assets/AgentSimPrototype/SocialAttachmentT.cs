/* =======================================================================================================
 * Social Attachment ADT
 * -------------------------------------------------------------------------------------------------------
 * The Social Attachment ADT is for defining how socially ``attached'' an entity is to A in discrete 
 * levels of liking.
 * 
 * The _levelUpSize and _levelDownSize state variables are for utility such that users can modify how many 
 * steps each ``attachment'' increment should take, which could differ in each direction. It is allowed to 
 * be negative to represent disliking A.
 * 
 * SocialAttachmentT can have child classes. All methods that change its internal state are marked 
 * "virtual" so that users can define their own implementations of them.
 * =======================================================================================================
 * Relies on: ---
 * =======================================================================================================
 * STATIC CODE METRICS SUMMARY
 * -------------------------------------------------------------------------------------------------------
 * Maintainability Index: 83
 * Cyclomatic Complexity: 13
 * Depth of Inheritance: 1
 * Class Coupling: 3
 * Lines of Source Code: 74
 * Lines of Executable Code: 23
 * =======================================================================================================
 */

using System;

namespace EMgine
{
    public class SocialAttachmentT
    {
        /* State Variables */
        private int _attachmentLevel;
        private int _levelUpSize;
        private int _levelDownSize;

        /* ---------------------------------------------------
         * METHODS
         * ---------------------------------------------------
         */

        /* Constructor */
        public SocialAttachmentT(int level)
        {
            _attachmentLevel = level;
            _levelUpSize = 1;
            _levelDownSize = 1;
        }

        /* Accessors */
        public int GetAttachmentLevel() => _attachmentLevel;
        public int GetAttachmentLevelUpSize() => _levelUpSize;
        public int GetAttachmentLevelDownSize() => _levelDownSize;

        /* Mutators */
        public virtual void ChangeAttachmentLevel(int level) => _attachmentLevel = level;

        public virtual void ChangeAttachmentLevelUpSize(int numLevels)
        {
            if (numLevels < 0) Utility.PrintMsg("W-SA_NEGATIVE_LEVELSIZE", Array.Empty<string>());
            _levelUpSize = Math.Max(0, numLevels);
        }

        public virtual void ChangeAttachmentLevelDownSize(int numLevels)
        {
            if (numLevels < 0) Utility.PrintMsg("W-SA_NEGATIVE_LEVELSIZE", Array.Empty<string>());
            _levelDownSize = Math.Max(0, numLevels);
        }

        /* 
         * 2022-12-06: Incrementing/decrementing _attachmentlevel past the min/max
         * value of int caused integer overflow. Added a try-catch block with
         * checked keyword to recover from it, added new warning message
         * Found By: MaximizeLevelUpSizeToForceOverflow
         *           MaximizeLevelDownSizeToForceOverflow
         */
        public virtual void IncrementAttachmentLevel()
        {
            try
            {
                checked { _attachmentLevel += _levelUpSize; }
            }
            catch(OverflowException)
            {
                Utility.PrintMsg("W-SA_OVERFLOW", Array.Empty<string>());
                _attachmentLevel = int.MaxValue;
            }
        }
        public virtual void DecrementAttachmentLevel()
        {
            try
            {
                checked { _attachmentLevel -= _levelDownSize; }
            }
            catch (OverflowException)
            {
                Utility.PrintMsg("W-SA_OVERFLOW", Array.Empty<string>());
                _attachmentLevel = int.MinValue;
            }
        }
        public virtual void AddToAttachmentLevel(int numLevels) => _attachmentLevel += numLevels;
        public virtual void ResetAttachmentLevelToZero() => _attachmentLevel = 0;
    }
}

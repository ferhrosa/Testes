using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalActionEngine
{
    public class MetalStage : MetalDrawableGameComponent
    {

        private List<MetalStageScreen> _screens;

        public List<MetalStageScreen> Screens
        {
            get
            {
                if ( _screens == null )
                    _screens = new List<MetalStageScreen>();

                return _screens;
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalActionEngine
{
    public class MetalStageScreen : MetalDrawableGameComponent
    {

        private List<MetalStagePart> _parts;

        public List<MetalStagePart> Parts
        {
            get
            {
                if ( _parts == null )
                    _parts = new List<MetalStagePart>();

                return _parts;
            }
        }




        public string Name { get; set; }

        public MetalStageScreen()
            : base()
        {

        }

    }
}

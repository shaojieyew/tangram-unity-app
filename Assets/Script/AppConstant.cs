using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    public class AppConstant
    {

        public static int PASS_THRESHOLD = 5;
        public static string RESULT_OBJ_TAG = "ResultShapes";
        public static string DRAGGABLE_OBJ_TAG = "Shapes";
        public static int DISTANCE_FROM_CAMERA_TO_OBJECT = 10;

        //merge grid point tgt
        public static double GRID_OVERLAP_TRESHOLD = 0.04;
        public static double AUTOFIT_DISTANCE_THRESHOLD = 0.3;
    }
}

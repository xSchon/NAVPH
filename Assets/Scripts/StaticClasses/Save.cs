/* Class used for saving game progress and its values */
using System.Collections.Generic;

class Save
{
    public string day;
    public float susMeterValue;
    public Dictionary<string, List<bool>> storyLines = new Dictionary<string, List<bool>>();
    public NestedStatus status;
}
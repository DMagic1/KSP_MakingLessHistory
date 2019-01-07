
using System.IO;
using UnityEngine;
using Expansions;

namespace MakingLessHistory
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class MakingLessHistory : MonoBehaviour
    {
        private const string MH_FOLDER = "MakingHistory/";
        private const string MH_EXPANSION_FILE = "makinghistory";
        private const string MH_FAKE_EXTENSION = ".notkspexpansion";
        
        private string _mhNewPath;

        private void Awake()
        {
            string expansionFolder = KSPExpansionsUtils.ExpansionsGameDataPath;

            string expansionPath = string.Concat(new string[] {
                expansionFolder
                , MH_FOLDER
                , MH_EXPANSION_FILE
                , ExpansionsLoader.expansionsMasterExtension });

            Debug.Log(string.Format("[MLH] Sandblasting Making History Expansion File: {0}", expansionPath));
            
            _mhNewPath = Path.ChangeExtension(expansionPath, MH_FAKE_EXTENSION);
            
            File.Move(expansionPath, _mhNewPath);
        }

        private void OnDestroy()
        {
            Debug.Log("[MLH] Restoring Making History Expansion File");

            string oldPath = Path.ChangeExtension(_mhNewPath, ExpansionsLoader.expansionsMasterExtension);

            File.Move(_mhNewPath, oldPath);
        }
    }
}

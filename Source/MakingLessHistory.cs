
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

        private bool _mhFileBlasted;

        private void Awake()
        {
            string expansionFolder = KSPExpansionsUtils.ExpansionsGameDataPath;

            string expansionPath = string.Concat(new string[] {
                expansionFolder
                , MH_FOLDER
                , MH_EXPANSION_FILE
                , ExpansionsLoader.expansionsMasterExtension });

            if (File.Exists(expansionPath))
            {
                Debug.Log(string.Format("[MLH] Sandblasting Making History Expansion File: {0}", expansionPath));

                _mhNewPath = Path.ChangeExtension(expansionPath, MH_FAKE_EXTENSION);

                File.Move(expansionPath, _mhNewPath);

                _mhFileBlasted = File.Exists(_mhNewPath);
            }
            else
            {
                string newExpansionPath = string.Concat(new string[] {
                    expansionFolder
                    , MH_FOLDER
                    , MH_EXPANSION_FILE
                    , MH_FAKE_EXTENSION });

                if (File.Exists(newExpansionPath))
                {
                    _mhNewPath = newExpansionPath;

                    _mhFileBlasted = File.Exists(_mhNewPath);

                    Debug.Log(string.Format("[MLH] Making History Expansion File Already Renamed: {0}\n Will Attempt To Reset After Loading...", _mhNewPath));
                }
                else
                {
                    Debug.Log(string.Format("[MLH] Cannot Locate Making History Expansion File [{0}]\nShutting Down...", expansionPath));

                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            if (!_mhFileBlasted)
                return;

            Debug.Log("[MLH] Restoring Making History Expansion File");

            string oldPath = Path.ChangeExtension(_mhNewPath, ExpansionsLoader.expansionsMasterExtension);

            File.Move(_mhNewPath, oldPath);
        }
    }
}

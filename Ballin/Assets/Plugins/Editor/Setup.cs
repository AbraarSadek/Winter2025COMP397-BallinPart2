using UnityEngine;
using UnityEditor;
using System.IO;

// Class Setup: This class contains methods to set up the folder structure for a Unity project.
public class Setup {

    //MenuItem Attribute - That adds a new menu option to Unity's toolbar called 'Setup' -> 'Create Folders'
    [MenuItem("Setup/Create Folders")]

    //CreateMyFolders Method - That creates a predefined set of folders in the Unity project
    public static void CreateMyFolders() {
        
        //Calls the CreateFolders method from the Folder class to create folders under the "_Project" directory
        Folder.CreateFolders("_Project", "Animations", "Art", "Audio", "Fonts",
            "Materials", "Prefabs", "ScriptableObjects", "Scripts", "Settings");

        AssetDatabase.Refresh(); //Refreshes the Unity Asset Database to ensure the new folders are recognized

    } //End of CreateMyFolders Method

    //Class Folder - Contains utility methods for folder creation.
    static class Folder {

        //CreateFolders Method - That create multiple folders under a root directory
        public static void CreateFolders(string root, params string[] folders) {

            //Combines the root folder name with the Unity project's data path to get the full path
            var fullPath = Path.Combine(Application.dataPath, root);

            //Foreach Loop - That iterates through each folder name provided in the params array
            foreach (string folder in folders) {

                // Combines the full path with the current folder name to get the folder's path
                var folderPath = Path.Combine(fullPath, folder);

                //If-Statement - That checks if the folder already exists, if not, it creates the folder
                if (!Directory.Exists(folderPath)) {
                    Directory.CreateDirectory(folderPath);
                } //End of if-statement

            } //End of Foreach loop

        } //End of CreateFolders Method

    } //End of Folder Class

} //End of Setup Class

using UnityEngine;

namespace FabrShooter
{
    [CreateAssetMenu(fileName = "Scene Data", menuName = "Configs/Scene Data")]
    public class SceneDataSO : ScriptableObject
    {
        [SerializeField] private int _mainMenuBuildIndex;
        [SerializeField] private int _mainLevelBuildIndex;
        [SerializeField] private int _testLevelBuildIndex;

        public int TestLevelBuildIndex => _testLevelBuildIndex;
        public int MainLevelBuildIndex => _mainLevelBuildIndex;
        public int MainMenuBuildIndex => _mainMenuBuildIndex;
    }
}
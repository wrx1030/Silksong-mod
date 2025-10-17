using System.Collections;
using UnityEngine;


namespace DeployBench
{ 
    public class Deploybench : MonoBehaviour
    {
        private GameObject benchClone;

        private void FindBenchClone()
        {
            foreach (Transform child in transform)
            {
                if (!child.gameObject.activeSelf && child.name.Contains("RestBench"))
                {
                    benchClone = child.gameObject;
                    Debug.Log($"[Deploybench] 找到 benchClone: {benchClone.name}");
                    return;
                }
            }

            benchClone = null;
        }


        private void Update()
        {
            
            if (Input.GetKeyDown(KeyCode.B))
            {
                FindBenchClone();


                if (benchClone == null)
                {
                    Debug.LogWarning($"[Deploybench] 未找到 benchClone，父对象: {name}");
                    return; 
                }

                TryDeployBench();
            }
        }

        private void TryDeployBench()
        {
            if (benchClone.activeSelf)
            {
                //string currentSceneName = GameManager.instance.sceneName;
            }

            StartCoroutine(DeployRoutine());
        }

        private IEnumerator DeployRoutine()
        {
            string currentSceneName = GameManager.instance.sceneName;
                
            benchClone.transform.SetParent(null);

            // 对齐位置到父物体
            var heroPos = this.transform.position;
            var pos = benchClone.transform.position;
            pos.x = heroPos.x;
            pos.y = heroPos.y + 0.3f;
            benchClone.transform.position = pos;
            Debug.Log($"[Deploybench] 已将 {benchClone.name} 对齐到位置: {pos.x},{pos.y}");

            benchClone.SetActive(true);
            Debug.Log("激活 benchClone");

            SceneTeleportMap.AddRespawnPoint(currentSceneName, benchClone.name);
            Debug.Log($"[Deploybench] 已在 {currentSceneName} 保存重生点: {benchClone.name}|{pos.x},{pos.y}");
            UnityEngine.Object.DontDestroyOnLoad(benchClone);
            Debug.Log($"[Deploybench] 已部署 benchClone: {benchClone.name}");

            // 添加冷却时间
            yield return new WaitForSeconds(0.5f);
        }
    }
}

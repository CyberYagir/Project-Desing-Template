using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Template.UI
{
    public class UICrossSceneFader : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image back;
        [SerializeField] private TMP_Text text;
        [SerializeField] private float fadeTime;
        public static bool isLoadScene;
        private void Awake()
        {
            canvas.enabled = true;
            back.DOFade(0, fadeTime).SetLink(back.gameObject).onComplete += () =>
            {
                canvas.enabled = false;
            };
            text.DOFade(0, fadeTime).SetLink(text.gameObject);
        }

        public void LoadScene(string id)
        {
            if (!isLoadScene)
            {
                isLoadScene = true;
                var operation = SceneManager.LoadSceneAsync(id);
                operation.allowSceneActivation = false;
                StartCoroutine(Animation(operation));
            }
        }

        IEnumerator Animation(AsyncOperation operation)
        {
            canvas.enabled = true;
            back.DOFade(1, fadeTime).SetLink(back.gameObject);
            text.DOFade(1, fadeTime).SetLink(text.gameObject);
            yield return new WaitForSeconds(fadeTime);
            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                {
                    isLoadScene = false;
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}

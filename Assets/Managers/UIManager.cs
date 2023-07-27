using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private EventSystem eventSystem;

    public Canvas canvasRoot;
    private Canvas popUpCanvas;
    private Stack<PopUpUI> popUpStack;
    public Stack<PopUpUI> popUp_Stack { get { return popUpStack; } }

    private void Awake()
    {
        // �� ����� Resource/UI ������ EventSystem �������� ����
        eventSystem = GameManager.Resource.Instantiate<EventSystem>("EventSystem");
        // �� ����ÿ��� EventSystem�� �����ǵ��� UIManager�� ���� �ڽ����� ����
        eventSystem.transform.SetParent(transform);

        canvasRoot = GameManager.Resource.Instantiate<Canvas>("Canvas");
        canvasRoot.transform.SetParent(transform);

        popUpCanvas = GameManager.Resource.Instantiate<Canvas>("Canvas");
        popUpCanvas.gameObject.name = "PopUpCanvas";
        popUpCanvas.transform.SetParent(transform);
        popUpCanvas.sortingOrder = 100;
        popUpStack = new Stack<PopUpUI>();
    }

    #region PopUpUI
    public T ShowPopUpUI<T>(T popUpUI) where T : PopUpUI
    {
        // ������ �� �˾��� ���� ��
        if (popUpStack.Count > 0)
        {
            // ���� UI�� ������ ���� ��ü�� Ȯ���� �� ��Ȱ��ȭ
            PopUpUI prevUI = popUpStack.Peek();
            prevUI.gameObject.SetActive(false);
        }

        T ui = GameManager.Pool.GetUI<T>(popUpUI);
        ui.transform.SetParent(popUpCanvas.transform, false);
        popUpStack.Push(ui);

        // �ð� ����
        Time.timeScale = 0f;
        return ui;
    }

    public T ShowPopUpUI<T>(string path) where T : PopUpUI
    {
        T ui = GameManager.Resource.Load<T>(path);
        return ShowPopUpUI(ui);
    }

    public void ClosePopUpUI()
    {
        PopUpUI ui = popUpStack.Pop();
        // ������ ���� �����ִ� ui�� �ݳ�
        GameManager.Pool.ReleaseUI(ui.gameObject);

        if (popUpStack.Count > 0)
        {
            PopUpUI currentUI = popUpStack.Peek();
            currentUI.gameObject.SetActive(true);
        }
        else
        {
            // Stack�� ����� ���(�˾� UI�� ��� ������ ���) �ð��� �帣���� ��
            Time.timeScale = 1f;
        }
    }
    #endregion PopUpUI
}
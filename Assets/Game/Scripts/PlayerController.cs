using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] List<Color> playerColorList;
    
    [SerializeField] float accelPower;      // ���ӷ�
    [SerializeField] float rotationSpeed;   // ȸ���ӵ�
    [SerializeField] float maxSpeed;        // �ִ�ӵ�
    [SerializeField] float fireCoolTime;    // �Ѿ� �߻� ��Ÿ��

    [SerializeField] Bullet bulletPrefab;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 inputDir;
    private float lastFireTime = float.MinValue;    // 0���� �ʱ�ȭ�ص� ��� �߻����� ���� �� ����

    [SerializeField] int count;
    [SerializeField] float hp;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        SetPlayerColor();

        // �ڽ��� �ƴ� �÷��̾��� Input�� �ı�
        if (!photonView.IsMine)
            Destroy(playerInput);

        if (photonView.IsMine)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 20, 0);
        }
    }

    private void Update()
    {
        Accelate(inputDir.y);   // �յڴ� ����
        Rotate(inputDir.x);     // �¿�� ȸ��
        Camera.main.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void Accelate(float input)
    {
        rb.AddForce(input * transform.forward * accelPower, ForceMode.Force);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void Rotate(float input)
    {
        transform.Rotate(Vector3.up, input * rotationSpeed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }
    
    private void OnFire(InputValue value)
    {
        // ��Ʈ��ũ���� �� ���ĸ� ���� �� ���� (�̸��� ���� ã��)
        photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient, transform.position, transform.rotation);
    }

    [PunRPC]
    private void RequestCreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        // Master Client ����(����)���� ������ ���� 
        if (Time.time < lastFireTime + fireCoolTime)
            return;

        lastFireTime = Time.time;

        float sentTime = (float)info.SentServerTime;
        photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, position, rotation, sentTime, info.Sender);
    }

    // ���� �Լ� ȣ��
    [PunRPC]
    private void ResultCreateBullet(Vector3 position, Quaternion rotation, float sentTime, Player player)
    {
        float lag = (float)(PhotonNetwork.Time - sentTime);

        Bullet bullet = Instantiate(bulletPrefab, position, rotation);
        bullet.SetPlayer(player);
        bullet.ApplyLag(lag);
    }

    private void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();
        if (playerColorList == null || playerColorList.Count <= playerNumber)
            return;

        Renderer render = GetComponent<Renderer>();
        render.material.color = playerColorList[playerNumber];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(count);
        }
        else // stream.IsReading
        {
            count = (int)stream.ReceiveNext();
        }
    }
}

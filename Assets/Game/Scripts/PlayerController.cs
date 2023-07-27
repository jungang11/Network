using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] List<Color> playerColorList;
    
    [SerializeField] float accelPower;      // 가속력
    [SerializeField] float rotationSpeed;   // 회전속도
    [SerializeField] float maxSpeed;        // 최대속도
    [SerializeField] float fireCoolTime;    // 총알 발사 쿨타임

    [SerializeField] Bullet bulletPrefab;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private Vector2 inputDir;
    private float lastFireTime = float.MinValue;    // 0으로 초기화해둘 경우 발사하지 않을 수 있음

    [SerializeField] int count;
    [SerializeField] float hp;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        SetPlayerColor();

        // 자신이 아닌 플레이어의 Input을 파괴
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
        Accelate(inputDir.y);   // 앞뒤는 가속
        Rotate(inputDir.x);     // 좌우는 회전
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
        // 네트워크에선 값 형식만 보낼 수 있음 (이름을 보내 찾기)
        photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient, transform.position, transform.rotation);
    }

    [PunRPC]
    private void RequestCreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        // Master Client 입장(서버)에서 판정을 진행 
        if (Time.time < lastFireTime + fireCoolTime)
            return;

        lastFireTime = Time.time;

        float sentTime = (float)info.SentServerTime;
        photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, position, rotation, sentTime, info.Sender);
    }

    // 원격 함수 호출
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

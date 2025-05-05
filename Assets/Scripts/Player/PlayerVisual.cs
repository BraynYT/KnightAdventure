using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private const string IS_RUNNING = "IsRunning";
    private const string IS_DIE = "IsDie";
    
    private PauseMenu pauseMenu;
    
    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += Player_OnPlayerDeath;
    }
    
    private void Update() {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        
        if (Player.Instance.IsAlive() && !pauseMenu.PauseGame)
            AdjustPlayerFacingDirection();
    }
    
    private void Player_OnPlayerDeath(object sender, System.EventArgs e)
    {
        animator.SetBool(IS_DIE, true);
        StartCoroutine(ShowDeathMenu());
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = GameInput.Instance.GetMousePosition();
        Vector3 playerPosition = Player.Instance.GetPlayerScreenPosition();

        if (mousePos.x < playerPosition.x) {
            spriteRenderer.flipX = true;
        } else {
            spriteRenderer.flipX = false;
        }
    }
    
    private IEnumerator ShowDeathMenu()
    {
        // Ждём завершения анимации смерти (предположим, она длится 1 секунду)
        yield return new WaitForSeconds(1f);
        
        // Показываем меню смерти
        pauseMenu.Pause();
    }
}

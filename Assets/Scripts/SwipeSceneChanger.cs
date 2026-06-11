using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch; 

public class ScreenSwiper : MonoBehaviour
{
    public RectTransform panelLinks;
    public RectTransform panelRechts;
    
    // Die Geschwindigkeit des Übergangs
    public float geschwindigkeit = 10f;

    // Zielpositionen für den Container
    private Vector3 zielPositionLinks;
    private Vector3 zielPositionRechts;
    private Vector3 aktuelleZielPos;

    void Start()
    {
        // Wir merken uns die Startpositionen
        zielPositionLinks = panelLinks.localPosition;
        
        // Wenn Panel_Rechts aktiv ist, muss Panel_Links nach links aus dem Bild geschoben werden
        // Wir berechnen den Abstand basierend auf der Breite des Panels
        float breite = panelLinks.rect.width;
        zielPositionRechts = zielPositionLinks - new Vector3(breite, 0, 0);

        // Standardmäßig starten wir links
        aktuelleZielPos = zielPositionLinks;
    }

    void Update()
    {

        if (Keyboard.current != null && Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            ZeigeLinks();
        }
        if (Keyboard.current != null && Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            ZeigeRechts();
        }
        // Bewege das linke Panel weich zum Ziel
        panelLinks.localPosition = Vector3.Lerp(panelLinks.localPosition, aktuelleZielPos, Time.deltaTime * geschwindigkeit);
        
        // Das rechte Panel bewegt sich synchron im gleichen Abstand mit
        float breite = panelLinks.rect.width;
        panelRechts.localPosition = panelLinks.localPosition + new Vector3(breite, 0, 0);
    }

    // Diese Methode rufst du auf, um nach Rechts zu wischen
    public void ZeigeRechts()
    {
        aktuelleZielPos = zielPositionRechts;
    }

    // Diese Methode rufst du auf, um nach Links zu wischen
    public void ZeigeLinks()
    {
        aktuelleZielPos = zielPositionLinks;
    }
}
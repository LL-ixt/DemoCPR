using UnityEngine;

public class CompressionMode : MonoBehaviour
{
    public ChessCompressed chessCompressed;
    public ActualCompressionBarFill actualCompressionBarFill;
    void OnEnable()
    {
        if (chessCompressed != null) chessCompressed.enabled = true;
        if (actualCompressionBarFill != null) actualCompressionBarFill.enabled = true;
    }
    void OnDisable()
    {
        if (chessCompressed != null) chessCompressed.enabled = false;
        if (actualCompressionBarFill != null) actualCompressionBarFill.enabled = false;
    }
}

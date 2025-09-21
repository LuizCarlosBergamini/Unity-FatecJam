using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song Chart", menuName = "Rhythm Game/Song Chart")]
public class SongChart : ScriptableObject {
    // The list of all notes in the song, defining the entire beatmap.
    public List<NoteController> notes;
    // The audio file for this song.
    public AudioClip songClip;
}

# UPDATE - Issue Resolved

This issue has been fixed in MediaElement v4.1.0. It was actually fixed in the main branch about a week before I ran into it.

# Problem playing audio from the internet using .Net MAUI's MediaElement on Android after screen lock

I'm having trouble playing internet hosted mp3 files on an Android device after the screen locks. After a few songs, MediaElement fails to play the next song, apparently with an UnknownHostException from ExoPlayer. 

This problem was discovered while developing a player app which sources the audio from AWS CloudFront. In this app, an API call is made to an Azure-hosted web app to get the URL of the next song. This call works. However, MediaElement/ExoPlayer fails. (The app works as expected on iOS.)

I created this barebones demonstration of the problem and used sample mp3 files from pixabay.com for simplicity. So, there should be no problem with the audio file hosting.

The debug log gives this information:
```
[BufferPoolAccessor2.0] bufferpool2 0x7a0b44d71198 : 5(40960 size) total buffers - 4(32768 size) used buffers - 14062/14067 (recycle/alloc) - 15/28080 (fetch/transfer)
[BufferPoolAccessor2.0] evictor expired: 1, evicted: 1
[ExoPlayerImplInternal] Playback error
[ExoPlayerImplInternal]   UnknownHostException (no network)
[CCodecBufferChannel] [c2.android.mp3.decoder#327] MediaCodec discarded an unknown buffer
[CCodecBufferChannel] [c2.android.mp3.decoder#327] MediaCodec discarded an unknown buffer
[CCodecBufferChannel] [c2.android.mp3.decoder#327] MediaCodec discarded an unknown buffer
[CCodecBufferChannel] [c2.android.mp3.decoder#327] MediaCodec discarded an unknown buffer
[hw-BpHwBinder] onLastStrongRef automatically unlinking death recipients
[BufferPoolAccessor2.0] bufferpool2 0x7a0b44d71198 : 0(0 size) total buffers - 0(0 size) used buffers - 14062/14067 (recycle/alloc) - 15/28080 (fetch/transfer)
[BufferPoolAccessor2.0] bufferpool2 0x7a0b44d71198 : 0(0 size) total buffers - 0(0 size) used buffers - 14062/14067 (recycle/alloc) - 15/28080 (fetch/transfer)
[BufferPoolAccessor2.0] evictor expired: 1, evicted: 1
```
All attempts to catch the UnknownHostException and explore further were fruitless. So, the exception must be consumed at a deeper level.

All specific code mentioned for MediaElement has been added per MediaElement's [documentation](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/maui/views/mediaelement?tabs=android).
Further, a wifilock is acquired to see if that helps. It didn't. I added a network-security-config.xml file to specifically allow pixabay.com traffic. This also didn't help.

I'm using both the default "Pixel 5 API 33" Android emulator and my Pixel 8 Pro.

I'm hoping someone with more experience can help me figure out what's going on here. I'm not sure if this is a bug in MediaElement or if I'm missing something.
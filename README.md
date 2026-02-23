# Game Server Binary Protocol (WebSocket)

This server uses **binary WebSocket messages** with a small header followed by a payload. The protocol is implemented with `.NET BinaryWriter/BinaryReader`, so all multi-byte numbers are **little-endian**.

> Important: `.NET BinaryWriter.Write(string)` uses a **7-bit encoded integer length prefix** (variable length), followed by the string bytes (UTF-8). So string fields do **not** have a fixed size.

---

## Common types (sizes)

| Type                 | Size (bytes) | Notes                                                                                           |
| -------------------- | ------------ | ----------------------------------------------------------------------------------------------- |
| `Int16`              | 2            | Used for all action codes and item/scene IDs                                                    |
| `Single` / `float32` | 4            | Used for coordinates                                                                            |
| `GuidString36`       | 36           | Fixed-length **ASCII** string (`Guid.ToString("D")`), written **without** a 7-bit length prefix |
| `string`             | `L + N`      | `L` = length prefix (7-bit encoded int, 1–5 bytes), `N` = UTF-8 byte count                      |

### `string` length prefix (`L`)

BinaryWriter encodes the string length using a 7-bit encoded integer:

- `0..127` bytes -> `L = 1`
- `128..16,383` bytes -> `L = 2`
- `16,384..2,097,151` bytes -> `L = 3`
- etc. (up to 5 bytes)

So the total size of a string field is:

```
string_size = length_prefix_bytes(L) + utf8_bytes(N)
```

> Note: `GuidString36` is **not** encoded using `BinaryWriter.Write(string)`. It is sent as raw 36 ASCII bytes with **no** 7-bit length prefix. See [`API.BinaryString36`](src/API/Utilities/BinaryString36.cs).

---

## Inbound messages (Client -> Server)

All inbound messages begin with:

- `actionCode: Int16` (2 bytes)

### 1) AddNewPlayerToLobby

**Layout**

- `[action:Int16][playerName:string]`

**Size**

- `2 + (L + N_playerName)`

**Read order (server)**

- `ReadInt16()` -> action
- `ReadString()` -> playerName

---

### 2) RemovePlayerFromLobby

**Layout**

- `[action:Int16]`

**Size**

- `2`

**Read order (server)**

- `ReadInt16()` -> action

---

### 3) TeleportPlayer

**Layout**

- `[action:Int16][x:Single][y:Single][sceneId:Int16]`

**Size**

- `2 + 4 + 4 + 2 = 12`

**Read order (server)**

- `ReadInt16()` -> action
- `ReadSingle()` -> x
- `ReadSingle()` -> y
- `ReadInt16()` -> sceneId

---

### 4) MovePlayer

**Layout**

- `[action:Int16][x:Single][y:Single]`

**Size**

- `2 + 4 + 4 = 10`

**Read order (server)**

- `ReadInt16()` -> action
- `ReadSingle()` -> x
- `ReadSingle()` -> y

---

## Outbound messages (Server -> Client)

All outbound messages begin with:

- `actionCode: Int16` (2 bytes)

These are built in [`src/API/BinaryMessageBuilder.cs`](src/API/BinaryMessageBuilder.cs).

### 1) PlayerJoinedLobby

**Layout**

- `[action:Int16][playerId:GuidString36][playerName:string]`

**Size**

- `2 + 36 + (L + N_playerName)`

---

### 2) PlayerLeftLobby

**Layout**

- `[action:Int16][playerId:GuidString36]`

**Size**

- `2 + 36`

---

### 3) PlayerMoved

**Layout**

- `[action:Int16][playerId:GuidString36][x:Single][y:Single]`

**Size**

- `2 + 36 + 4 + 4 = 46`

---

### 4) PlayerTeleported

**Layout**

- `[action:Int16][playerId:GuidString36][x:Single][y:Single][scene:Int16]`

**Size**

- `2 + 36 + 4 + 4 + 2 = 48`

---

### 5) Error

**Layout**

- `[action:Int16][errorMessage:string]`

**Size**

- `2 + (L + N_errorMessage)`

---

### 6) Success

**Layout**

- `[action:Int16][successMessage:string]`

**Size**

- `2 + (L + N_successMessage)`

---

## Notes / Gotchas

1. **Action code width must match.**  
   Both inbound and outbound action codes are `Int16` (2 bytes). Do not write them as `Int32` (4 bytes), or all subsequent reads will be offset/wrong.

2. **`playerId` is fixed-length and has NO length prefix.**  
   `playerId` is encoded as exactly **36 ASCII bytes** (Guid `"D"` format) and written/read via [`API.BinaryString36`](src/API/Utilities/BinaryString36.cs), not `BinaryWriter.Write(string)` / `BinaryReader.ReadString()`.

3. **Other strings are temporarily variable-length.**  
   `playerName`, `successMessage`, etc. use the standard `.NET BinaryWriter` string encoding (7-bit length prefix + UTF-8). This will be changed in the future.

---

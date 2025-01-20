# How can I use PatosMod/How hard is it is to program?

PatosMod is not exactly an engine; it's more than a framework where complex/system functions (such as opening an browser tab) can be called using simple functions.

## Example:

Things like _opening an MessageBox_ are **NOT** on “pure” Lua, so it makes using PatosMod better than using a bunch of modules to make a simple action:

**PatosMod:**
``` lua
MessageBox('Hello world!')
```

“Pure” Lua: [Source](https://stackoverflow.com/a/32920678/26411616)
``` lua
local ffi = require("ffi")  -- Load FFI module (instance)

local user32 = ffi.load("user32")   -- Load User32 DLL handle

ffi.cdef([[
enum{
    MB_OK = 0x00000000L,
    MB_ICONINFORMATION = 0x00000040L
};

typedef void* HANDLE;
typedef HANDLE HWND;
typedef const char* LPCSTR;
typedef unsigned UINT;

int MessageBoxA(HWND, LPCSTR, LPCSTR, UINT);
]]) -- Define C -> Lua interpretation

user32.MessageBoxA(nil, "Hello world!", "My message", ffi.C.MB_OK + ffi.C.MB_ICONINFORMATION)   -- Call C function 'MessageBoxA' from User32

```

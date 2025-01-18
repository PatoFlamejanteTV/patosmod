-- wait.lua module, module by UltimateQuack
local wait = {}

-- Function to wait for a specified number of seconds
function wait.seconds(seconds)
    local start = os.time()
    repeat until os.time() > start + seconds
end

return wait
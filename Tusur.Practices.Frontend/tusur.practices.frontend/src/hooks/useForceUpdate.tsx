import { useState } from "react";

function useForceUpdate() {
    const [_, setValue] = useState(0);
    return () => setValue(value => value + 1);
}

export default useForceUpdate;
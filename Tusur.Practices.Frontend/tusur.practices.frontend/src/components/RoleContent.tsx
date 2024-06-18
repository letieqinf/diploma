import useAuth from "../hooks/useAuth";

function RoleContent({ children, roles }) {
    const { auth } = useAuth();

    return (
        <>
            { auth?.roles.some(role => roles?.includes(role))
                ? <>{ children }</>
                : <></>
            }
        </>
    );
}

export default RoleContent;
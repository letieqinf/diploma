import { useLocation, Navigate, Outlet } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import AuthorizedHeader from "../pages/components/layout/headers/AuthorizedHeader";

function RequireAuth({ allowedRoles }) {
    const { auth } = useAuth();
    const location = useLocation();

    if (!auth?.accessToken) {
        return (
            <Navigate 
                to="/login" 
                state={{ from: location }}
                replace
            />
        );
    }

    return (
        auth?.roles.some(role => allowedRoles?.includes(role))
            ?   <>
                    <AuthorizedHeader/>
                    <Outlet />
                </>
            :   <Navigate
                    to="/unauthorized"
                    state={{ from: location }}
                    replace
                />
    );
}

export default RequireAuth;
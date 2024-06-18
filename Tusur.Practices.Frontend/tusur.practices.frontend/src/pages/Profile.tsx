import { useState, useEffect, useRef } from "react";
import { useNavigate, useLocation } from "react-router-dom";

import useAxiosPrivate from "../hooks/useAxiosPrivate";

import './Profile.css';

function Profile() {
    const [profileContent, setProfileContent] = useState({
        lastName: '',
        firstName: '',
        patronymic: '',
        email: ''
    });

    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();
    const location = useLocation();

    const effectRan = useRef(false);

    useEffect(() => {
        let isMounted = true;
        const controller = new AbortController();
        const signal = controller.signal;
        
        const getProfile = async () => {
            try {
                const response = await axiosPrivate.get('/api/account/profile', {
                    signal: signal
                });
                isMounted && setProfileContent({
                    lastName: response.data.lastName,
                    firstName: response.data.name,
                    patronymic: response.data.patronymic,
                    email: response.data.email
                });
            } catch (error) {
                navigate(-1); // , { state: { from: location }, replace: true }
            }
        }
        
        if (effectRan.current)
            getProfile();
    
        return () => {
            isMounted = false;
            controller.abort();
            effectRan.current = true;
        }     
    }, []);

    return (
        <>
            <div className="profile-container">
                <div className="profile-card">
                    <div className="profile-photo">
                        <div className="photo-container" />
                    </div>
                    <div className="profile-body">
                        <div className="profile-header">
                            <h3>Профиль</h3>
                        </div>
                        <div className="profile-information">
                            <p>{profileContent.lastName} {profileContent.firstName} {profileContent.patronymic}</p>
                            <p>{profileContent.email}</p>
                        </div>
                    </div>
                    <div className="profile-practices">

                    </div>
                </div>
            </div>
        </>
    );
}

export default Profile;
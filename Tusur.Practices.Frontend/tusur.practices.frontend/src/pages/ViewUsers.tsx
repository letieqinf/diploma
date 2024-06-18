import { useEffect, useState } from "react";
import { useNavigate, Link } from "react-router-dom";

import { Roles } from "../components/Roles";

import useAxiosPrivate from "../hooks/useAxiosPrivate";

function ViewUsers() {
    const [users, setUsers] = useState([{
        id: '',
        email: '',
        name: '',
        lastName: '',
        patronymic: ''
    }]);

    const [roleFilter, setRoleFilter] = useState(0);

    const [cardState, setCardState] = useState(false);
    const [cardInfo, setCardInfo] = useState({
        email: '',
        name: '',
        lastName: '',
        patronymic: ''
    });

    const axiosPrivate = useAxiosPrivate();
    const navigate = useNavigate();

    const handleClick = () => {
        setRoleFilter(roleFilter === 4 ? 0 : roleFilter + 1)
    }

    const handleCardOpen = async (event) => {
        let forCard = users.find(user => `user-${user.id}` === event.target.id);
        if (!forCard)
            return;

        try {
            const response = await axiosPrivate.get(`/api/tools/users/${forCard.id}/roles`);
            console.log(response.data);
        } catch (error) {
            console.error(error);
            return;
        }

        setCardState(true);
        setCardInfo({
            email: forCard.email,
            name: forCard.name,
            lastName: forCard.lastName,
            patronymic: forCard.patronymic
        });
    }

    useEffect(() => {
        const getUsers = async () => {
            try {
                const response = await axiosPrivate.get(`/api/tools/users${'?role=' + Roles[roleFilter]}`);
                setUsers(response.data);
            } catch (error) {
                console.error(error);
                navigate(-1);
            }
        }
        getUsers();
    }, [roleFilter]);

    return (
        <>
            <button onClick={handleClick}>Следующая роль</button>
            <ul>
                {
                    users.map((user, index) => 
                        <li id={'user-' + user.id} key={index} onClick={handleCardOpen}>
                            { user.lastName } { user.name } { user.patronymic }
                        </li>
                    )
                }
            </ul>
            <div className="add-to-role-card">
                <div className="card-header">
                    <h3>Информация</h3>
                </div>
                <div className="card-content">

                </div>
            </div>
        </>
    );
}

export default ViewUsers;
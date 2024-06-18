import { useEffect, useState } from 'react';
import RoleContent from '../components/RoleContent';

import './Home.css';
import './HomeModal.css';
import Student from './components/home/Student';
import { Teacher } from './components/home/teacher/Teacher';
import { Education } from './components/home/education/Education';

function Home() {
    const [tab, setTab] = useState<number>(0);

    useEffect(() => {
        const tabNames = document.getElementsByClassName('role-tab');
        const tabContents = document.getElementsByClassName('role-tabs-content');

        if (tabNames.length !== 0 && tabContents.length !== 0) {
            tabNames[tab].classList.add('active');
            tabContents[tab].classList.add('active');
        }
    }, []);

    const onTabClick = (event: React.SyntheticEvent): void => {
        if (event.target instanceof Element) {
            let target = event.target;

            let tabNames = document.getElementsByClassName('role-tab');
            let tabContents = document.getElementsByClassName('role-tabs-content');

            if (tabNames.length !== 0 && tabContents.length !== 0) {
                let index = Array.prototype.slice.call(tabNames).indexOf(target);
                if (index === -1)
                    return;

                tabNames[tab].classList.remove('active');
                tabContents[tab].classList.remove('active');
                
                tabNames[index].classList.add('active');
                tabContents[index].classList.add('active');

                setTab(index);
            }
        }
    };

    return (
        <>
            <article className="home-body">
                <div className="container">
                    <div className="role-tabs-container">
                        <ul className="role-tabs">
                            <RoleContent roles={['student']}>
                                <li className="role-tab" onClick={onTabClick}>
                                    Студентам
                                </li>
                            </RoleContent>
                            <RoleContent roles={['teacher']}>
                                <li className="role-tab" onClick={onTabClick}>
                                    Преподавателям
                                </li>
                            </RoleContent>
                            <RoleContent roles={['education']}>
                                <li className="role-tab" onClick={onTabClick}>
                                    Центру Карьеры
                                </li>
                            </RoleContent>
                            <RoleContent roles={['secretary']}>
                                <li className="role-tab" onClick={onTabClick}>
                                    Секретарям
                                </li>
                            </RoleContent>
                        </ul>
                    </div>
                    <div className="role-tabs-content-container">
                        <RoleContent roles={['student']}>
                            <div className="role-tabs-content">
                                <Student />
                            </div>
                        </RoleContent>
                        <RoleContent roles={['teacher']}>
                            <div className="role-tabs-content">
                                <Teacher />
                            </div>
                        </RoleContent>
                        <RoleContent roles={['education']}>
                            <div className={ `role-tabs-content` }>
                                <Education />
                            </div>
                        </RoleContent>
                    </div>
                </div>
            </article>
        </>
    );
}

export default Home;
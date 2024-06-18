import { useState, useEffect } from "react";
import { Link } from "react-router-dom";

import './Dropdown.css';

function Dropdown(props: {children: React.ReactNode, title: string, link: string}) {
    const [isChecked, setIsChecked] = useState(false);

    const handleMouseEnter = (event) => {
        let parent = event.target.parentElement;
        console.log(parent.children);
        setIsChecked(true)
    }

    const handleMouseLeave = (event) => {
        console.log(event.target.parentElement);
        setIsChecked(false)
    }

    return (
        <li
            // onMouseEnter={handleMouseEnter}
            onMouseLeave={handleMouseLeave}
            onMouseOver={handleMouseEnter}
        >
            <Link to={ props.link }>
                { props.title }
            </Link>
            {
                isChecked
                    ?   <ul className="toggle-menu">
                            { Array.isArray(props.children)
                                ? props.children.map(child => <>{ child }</>)
                                : <>{ props.children }</>}
                        </ul>
                    :   <></>
            }
        </li>
    );
}

export default Dropdown;
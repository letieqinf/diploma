import { Link } from "react-router-dom";

function DropdownItem(props: {title: string, link: string}) {
    return (
        <li className="toggle-menu-element">
            <Link to={ props.link }>{ props.title }</Link>
        </li>
    );
}

export default DropdownItem;
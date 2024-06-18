import { IconProp } from "@fortawesome/fontawesome-svg-core";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import './Contents.css';

function Contents(props: { title: string, icon: IconProp, children: React.ReactNode }) {
    return (
        <div className="contents-card">
            <div className="header">
                <FontAwesomeIcon icon={ props.icon } className="header-svg" />
                <h3 className="header-title">{ props.title }</h3>
            </div>
            <ul className="body">
                { Array.isArray(props.children)
                    ? props.children.map(child => <li className="body-element">{ child }</li>)
                    : <li className="body-element">{ props.children }</li> }
            </ul>
        </div>
    );
}

export default Contents;
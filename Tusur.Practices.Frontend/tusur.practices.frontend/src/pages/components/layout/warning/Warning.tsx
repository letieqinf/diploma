import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "./Warning.css";
import { faWarning } from "@fortawesome/free-solid-svg-icons";

interface IWarning {
    title: string,
    className?: string
}

export const Warning: React.FunctionComponent<IWarning> = ( props ) => {
    const { title, className } = props;

    return (
        <div className={ `warning ${ className }` }>
            <FontAwesomeIcon icon={ faWarning } />
            <p>{ title }</p>
        </div>
    );
}
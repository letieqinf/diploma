import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import { Children, useEffect, useRef, useState } from "react";

import "./Organizer.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

interface IOrganizer {
    children: React.ReactElement<IOrganizerItem> | Array<React.ReactElement<IOrganizerItem>>,
    currentValue: any | undefined,
    onChange: Function
};

export const Organizer: React.FunctionComponent<IOrganizer> = ( props ) => {
    const children = Children.toArray(props.children) as Array<React.ReactElement<IOrganizerItem>>;

    const [toShow, setToShow] = useState<number>();
    const [title, setTitle] = useState<string | undefined>();

    const onLoad = useRef(false);

    useEffect(() => {
        if (onLoad.current) {
            const child = children.findIndex(item => item.props.value === props.currentValue);
            if (child !== -1) {
                setToShow(child);
            }
        }

        onLoad.current = true;
    }, [props.currentValue])

    useEffect(() => {
        const defineTitle = (parentValue: any | undefined, title: string | undefined): string | undefined => {
            if (!parentValue)
                return title;
                        
            const parent = children.find(child => child.props.value === parentValue);
            const newTitle = `${ parent?.props.title } / ${ title }`;
            return defineTitle(parent?.props.parentValue, newTitle);
        }

        if (toShow !== undefined) {
            const child = children[toShow];
            setTitle(defineTitle(child.props.parentValue, child.props.title));
        }
    }, [toShow])

    const handleBackClick = () => {
        if (toShow !== undefined) {
            const child = children.findIndex(item => item.props.value === children[toShow].props.parentValue);
            if (child !== -1)
                props.onChange(children[child].props.value);
        }
    }

    return (
        <>
            <div className={ `organizer-title` }>
                {
                    title ?
                        <>
                            <FontAwesomeIcon icon={ faChevronLeft } onClick={ handleBackClick } />
                            <p>{ title }</p>
                        </>
                    : <></>
                }
            </div>
            <div>
                {
                    toShow !== undefined ?
                        children[toShow]
                    : <></>
                }
            </div>
        </>
    );
}

interface IOrganizerItem {
    children: React.ReactNode | Array<React.ReactNode>,
    value: any,
    parentValue?: any,
    title?: string
}

export const OrganizerItem: React.FunctionComponent<IOrganizerItem> = ( props ) => {
    return (
        <>{ props.children }</>
    );
};